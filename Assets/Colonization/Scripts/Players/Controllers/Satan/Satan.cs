using System;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Satan : Player, IReactive<Satan>
    {
        protected readonly SatanAbilities _parameters;
        protected readonly SatanLeveling _leveling;
        protected readonly Artefact _artefact;

        protected readonly DemonsSpawner _spawner;

        protected readonly VAction<Satan> _eventChanged = new();

        protected int _curse, _maxCurse;

        public int Level { [Impl(256)] get => _leveling.Level; }
        public int MaxLevel { [Impl(256)] get => _leveling.MaxLevel; }
        public int Curse { [Impl(256)] get => _curse / _parameters.maxCursePerLevel; }
        public int MaxCurse { [Impl(256)] get => _maxCurse / _parameters.maxCursePerLevel; }
        public float CursePercent { [Impl(256)] get => (float)_curse/_maxCurse; }

        protected Satan(Settings settings) : base(PlayerId.Satan)
        {
            var storage = GameContainer.Storage.Satan;
            _parameters = SettingsFile.Load<SatanAbilities>();

            var loadData = storage.LoadData;

            _curse = loadData.state.curse;
            _maxCurse = _parameters.maxCurseBase + loadData.state.level * _parameters.maxCursePerLevel;

            _leveling = new(settings.satanLeveling, loadData.state.level);
            _artefact = Artefact.Create(settings.artefact, loadData);

            _spawner = new(new(PlayerId.Satan, _leveling, _artefact), GameContainer.Hexagons[Key.Zero], loadData.state.spawn);

            for (int i = loadData.actors.Count - 1; i >= 0; i--)
                _spawner.Load(loadData.actors[i]);

            storage.StateBind(this, !loadData.isLoaded);
            storage.BindArtefact(_artefact, !loadData.isLoaded);
            storage.BindActors(Actors);
            storage.LoadData = null;
        }

        public Subscription Subscribe(Action<Satan> action, bool instantGetValue) => _eventChanged.Add(action, instantGetValue, this);

        [Impl(256)] protected void LevelUp()
        {
            if(_leveling.Next())
                _maxCurse = _parameters.maxCurseBase + _leveling.Level * _parameters.maxCursePerLevel;

            _curse -= _maxCurse;
            _spawner.AddPotential(Math.Min(_parameters.maxPotentialPerLvl, _leveling.Level));
            GameContainer.Chaos.ForSatanLevelUP();
        }
    }
}
