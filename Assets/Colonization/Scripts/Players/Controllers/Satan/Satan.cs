using System;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class Satan : Player, IReactive<Satan>
    {
        protected readonly RInt _level;
        protected readonly RInt _curse;
        
        protected readonly SatanAbilities _settings;

        protected readonly DemonLeveling _leveling;
        protected readonly Artefact _artefact;

        protected readonly DemonsSpawner _spawner;

        protected readonly Subscription<Satan> _eventChanged = new();


        public IReactiveValue<int> Level => _level;
        public IReactiveValue<int> Curse => _curse;

        public int MaxCurse => _settings.maxCurse + _level * _settings.maxCursePerLevel;

        protected Satan(Settings settings) : base(PlayerId.Satan)
        {
            var storage = GameContainer.Storage.Satan;
            _settings = SettingsFile.Load<SatanAbilities>();

            var loadData = storage.LoadData;

            _level = new(loadData.state.level);
            _curse = new(loadData.state.curse);

            _leveling = new(settings.demonLeveling, _level);
            _artefact = Artefact.Create(settings.artefact, loadData);

            _spawner = new(_level, new(PlayerId.Satan, _leveling, _artefact), GameContainer.Hexagons[Key.Zero], loadData.state.spawn);

            for (int i = loadData.actors.Count - 1; i >= 0; i--)
                _spawner.Load(loadData.actors[i]);

            storage.StateBind(this, !loadData.isLoaded);
            storage.BindArtefact(_artefact, !loadData.isLoaded);
            storage.BindActors(Actors);
            storage.LoadData = null;
        }

        public Unsubscription Subscribe(Action<Satan> action, bool instantGetValue) => _eventChanged.Add(action, instantGetValue, this);

        protected void AddCurse(int value)
        {
            _curse.Add(value);

            int maxCurse = MaxCurse;
            if (_curse >= maxCurse)
            {
                _curse.Remove(maxCurse);
                _level.Increment();
            }

            _eventChanged.Invoke(this);
        }
    }
}
