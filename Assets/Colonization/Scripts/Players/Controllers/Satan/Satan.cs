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
        
        protected readonly SatanAbilities _states;
        protected readonly Balance _balance;

        protected readonly DemonLeveling _leveling;
        protected readonly Artefact _artefact;

        protected readonly DemonsSpawner _spawner;

        protected readonly Subscription<Satan> _eventChanged = new();


        public IReactiveValue<int> Level => _level;
        public IReactiveValue<int> Curse => _curse;

        public int MaxCurse => _states.maxCurse + _level * _states.maxCursePerLevel;
        public int CursePerTurn
        {
            get
            {
                int ratio = (_balance > 0 ? _states.ratioPenaltyCurse : _states.ratioRewardCurse) >> SatanAbilities.SHIFT_RATIO;
                return _states.cursePerTurn - _balance * ratio;
            }
        }

        protected Satan(Settings settings) : base(PlayerId.Satan, CONST.DEFAULT_MAX_DEMONS)
        {
            var storage = GameContainer.Storage.Satan;
            _states = SettingsFile.Load<SatanAbilities>();

            var loadData = storage.LoadData;

            _level = new(loadData.state.level);
            _curse = new(loadData.state.curse);

            _leveling = new(settings.demonLeveling, _level);
            _artefact = Artefact.Create(settings.artefact, loadData);

            _spawner = new(_level, new(_leveling, _artefact), GameContainer.Hexagons[Key.Zero], loadData.state.spawn);

            for (int i = loadData.actors.Count - 1; i >= 0; i--)
                _actors.Add(_spawner.Load(loadData.actors[i]));

            _balance = GameContainer.Balance;
            _balance.BindDemons(_actors);

            storage.StateBind(this, !loadData.isLoaded);
            storage.BindArtefact(_artefact, !loadData.isLoaded);
            storage.BindActors(_actors);
            storage.LoadData = null;

            SpellBook.AddSatanActors(_actors);
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
