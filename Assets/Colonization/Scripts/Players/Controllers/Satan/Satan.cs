using System;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Satan : Player, IReactive<Satan>
    {
        protected static readonly SatanAbilities s_parameters;
        protected readonly SatanLeveling _leveling;
        protected readonly Artefact _artefact;

        protected readonly Spawner _spawner;

        protected readonly VAction<Satan> _eventChanged = new();

        protected int _curse, _maxCurse;

        public int Level { [Impl(256)] get => _leveling.Level; }
        public int MaxLevel { [Impl(256)] get => _leveling.MaxLevel; }
        public int Curse { [Impl(256)] get => _curse / s_parameters.maxCursePerLevel; }
        public int MaxCurse { [Impl(256)] get => _maxCurse / s_parameters.maxCursePerLevel; }
        public float CursePercent { [Impl(256)] get => (float)_curse/_maxCurse; }

        static Satan() => s_parameters = SettingsFile.Load<SatanAbilities>();

        protected Satan(Settings settings) : base(PlayerId.Satan, false)
        {
            var storage = GameContainer.Storage.Satan;
            var loadData = storage.LoadData;

            _curse = loadData.state.curse;
            _maxCurse = s_parameters.maxCurseBase + loadData.state.level * s_parameters.maxCursePerLevel;

            _leveling = new(settings.satanLeveling, loadData.state.level);
            _artefact = Artefact.Create(settings.artefact, loadData);

            _spawner = new(new(PlayerId.Satan, _leveling, _artefact), loadData.state.spawn);

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
                _maxCurse = s_parameters.maxCurseBase + _leveling.Level * s_parameters.maxCursePerLevel;

            _curse -= _maxCurse;
            _spawner.AddPotential(Math.Min(s_parameters.maxPotentialPerLvl, 1 + (_leveling.Level >> 1)));
            GameContainer.Chaos.ForSatanLevelUP();
        }
    }
}
