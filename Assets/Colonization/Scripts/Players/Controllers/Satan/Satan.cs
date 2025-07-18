using System;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class Satan : Player, IReactive<Satan>
    {
        protected readonly RInt _level;
        protected readonly RInt _curse;
        
        protected readonly SatanAbilities _states;

        protected readonly DemonLeveling _leveling;
        protected readonly Artefact _artefact;

        protected readonly DemonsSpawner _spawner;
        protected readonly ReactiveSet<Actor> _demons;

        protected readonly Subscription<Satan> _eventChanged = new();
        protected readonly Unsubscriptions _unsubscribers = new();

        public IReactiveValue<int> Level => _level;
        public IReactiveValue<int> Curse => _curse;

        public ReadOnlyReactiveSet<Actor> Demons => _demons;

        public int MaxCurse => _states.maxCurse + _level * _states.maxCursePerLevel;
        public int CursePerTurn
        {
            get
            {
                int ratio = (s_states.balance > 0 ? _states.ratioPenaltyCurse : _states.ratioRewardCurse) >> SatanAbilities.SHIFT_RATIO;
                return _states.cursePerTurn - s_states.balance * ratio;
            }
        }

        protected Satan(Settings settings) : base(PlayerId.Satan)
        {
            var storage = settings.storage.Satan;
            _states = SettingsFile.Load<SatanAbilities>();

            var loadData = storage.LoadData;

            _level = new(loadData.state.level);
            _curse = new(loadData.state.curse);

            _leveling = new(settings.demonLeveling, _level);
            _artefact = Artefact.Create(settings.artefact, loadData);

            _spawner = new(_level, new(_leveling, _artefact), settings, s_hexagons[Key.Zero], loadData.state.spawn);

            _demons = new(loadData.state.maxDemons);
            for (int i = loadData.actors.Count - 1; i >= 0; i--)
                _demons.Add(_spawner.Load(loadData.actors[i], s_hexagons));

            s_states.balance.BindDemons(_demons);

            storage.StateBind(this, !loadData.isLoaded);
            storage.BindArtefact(_artefact, !loadData.isLoaded);
            storage.BindActors(_demons);
            storage.LoadData = null;

            SpellBook.AddSatan(this);
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

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
            _demons.Dispose();
        }
    }
}
