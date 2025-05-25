using System;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class Satan : IPlayerController, IReactive<Satan>
    {
        private readonly RInt _level;
        private readonly RInt _curse;
        private readonly Balance _balance;
        
        private readonly SatanAbilities _states;

        private readonly DemonBuffs _leveling;
        private readonly Buffs _artefact;

        private readonly DemonsSpawner _spawner;
        private readonly ReactiveSet<Actor> _demons;

        private readonly Subscription<Satan> _eventChanged = new();
        private readonly Unsubscriptions _unsubscribers = new();

        private readonly Action endTurn;

        public IReactiveValue<int> Level => _level;
        public IReactiveValue<int> Curse => _curse;
        public IReactive<int> Balance => _balance;

        public int MaxCurse => _states.maxCurse + _level * _states.maxCursePerLevel;
        public int CursePerTurn
        {
            get
            {
                int ratio = (_balance > 0 ? _states.ratioPenaltyCurse : _states.ratioRewardCurse) >> SatanAbilities.SHIFT_RATIO;
                return _states.cursePerTurn - _balance * ratio;
            }
        }

        public Satan(SatanStorage storage, Players.Settings settings, Action endTurn)
        {
            _states = SettingsFile.Load<SatanAbilities>();

            var loadData = storage.LoadData;

            _level = new(loadData.state.level);
            _curse = new(loadData.state.curse);
            _balance = settings.balance;

            _leveling = new(settings.demonBuffs.Settings, _level);
            _artefact = Buffs.Create(settings.artefact.Settings, loadData);

            _spawner = new(_level, new(_leveling, _artefact), settings, loadData.state.spawn);

            _demons = new(loadData.state.maxDemons);
            _demons.Subscribe((actor, evt) => { if (evt == TypeEvent.Add) actor.OnKilled.Add(ActorKill); }, false);
            for (int i = loadData.actors.Count - 1; i >= 0; i--)
                _demons.Add(_spawner.Load(loadData.actors[i], settings.hexagons));

            storage.StateBind(this, !loadData.isLoaded);
            storage.BindArtefact(_artefact, !loadData.isLoaded);
            storage.BindActors(_demons);

            storage.LoadData = null;
            
            
            this.endTurn = endTurn;
        }

        public Unsubscription Subscribe(Action<Satan> action, bool instantGetValue) => _eventChanged.Add(action, instantGetValue, this);

        public void OnInit()
        {
            endTurn();
        }

        public void OnEndTurn()
        {
            int countBuffs = 0, balance = 0;
            foreach(var demon in _demons)
            {
                if (demon.IsMainProfit)
                    balance += (demon.Id + 1);
                if (demon.IsAdvProfit)
                    countBuffs++;

                demon.StatesUpdate();
            }

            _balance.DemonCurse(balance);
            _artefact.Next(countBuffs);
        }

        public void OnProfit(Id<PlayerId> id, int hexId)
        {
            if (hexId == CONST.GATE_ID)
                AddCurse(_states.curseProfit + _level * _states.curseProfitPerLevel);
        }

        public void OnStartTurn()
        {
            foreach (var demon in _demons)
                demon.EffectsUpdate(_states.gateDefense);

            AddCurse(CursePerTurn);
        }

        public void OnPlay()
        {
        }

        private void AddCurse(int value)
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

        private void ActorKill(Id<PlayerId> target, int actorId)
        {
            UnityEngine.Debug.Log($"ActorKilling: {target}, {actorId}");
        }
    }
}
