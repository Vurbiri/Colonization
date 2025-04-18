//Assets\Colonization\Scripts\Players\Satan\Satan.cs
using System;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class Satan : IReactive<Satan>
    {
        private readonly RInt _level;
        private readonly RInt _curse;
        private readonly RInt _balance;
        
        private readonly SatanAbilities _states;

        private readonly DemonBuffs _leveling;
        private readonly Buffs _artefact;

        private readonly DemonsSpawner _spawner;
        private readonly ReactiveSet<Actor> _demons;

        private readonly Signer<Satan> _eventSelf = new();
        private readonly Signer<Win> _eventWin = new();
        private readonly Unsubscribers _unsubscribers = new();

        public IReactiveValue<int> Level => _level;
        public IReactiveValue<int> Curse => _curse;
        public IReactiveValue<int> Balance => _balance;

        public int MaxCurse => _states.maxCurse + _level * _states.maxCursePerLevel;
        public int CursePerTurn
        {
            get
            {
                int ratio = (_balance > 0 ? _states.ratioPenaltyCurse : _states.ratioRewardCurse) >> SatanAbilities.SHIFT_RATIO;
                return _states.cursePerTurn - _balance * ratio;
            }
        }

        public Satan(SatanStorage storage, Players.Settings settings, Hexagons hexagons, IReadOnlyList<Human> humans)
        {
            _states = settings.satanStates;

            var loadData = storage.LoadData;

            _level = new(loadData.state.level);
            _curse = new(loadData.state.curse);
            _balance = new(loadData.state.balance);

            _leveling = new(settings.demonBuffs.Settings, _level);
            _artefact = Buffs.Create(settings.artefact.Settings, loadData);

            _spawner = new(_level, new(_leveling, _artefact), settings, hexagons[Key.Zero], loadData.state.spawn);

            _demons = new(loadData.state.maxDemons);
            for (int i = loadData.actors.Count - 1; i >= 0; i--)
                _demons.Add(_spawner.Load(loadData.actors[i], hexagons));

            for (int i = 0; i < PlayerId.HumansCount; i++)
            {
                _unsubscribers += humans[i].Perks.Subscribe(OnAddPerk, false);
                _unsubscribers += humans[i].Shrines.Subscribe(OnAddShrine, false);
            }

            _unsubscribers += SceneContainer.Get<GameplayEventBus>().EventActorKilling + ActorKilling;

            storage.StateBind(this, !loadData.isLoaded);
            storage.ArtefactBind(_artefact, !loadData.isLoaded);
            storage.ActorsBind(_demons);

            storage.LoadData = null;

            #region Local: OnPerk(..), OnShrineBuild(..)
            //=================================
            void OnAddPerk(Perk perk) => AddBalance(_states.balancePerPerk);
            //=================================
            void OnAddShrine(int index, Crossroad crossroad, TypeEvent type)
            {
                if (type == TypeEvent.Add) AddBalance(_states.balancePerShrine);
            }
            #endregion
        }

        public Unsubscriber Subscribe(Action<Satan> action, bool instantGetValue) => _eventSelf.Add(action, instantGetValue, this);

        public void EndTurn()
        {
            int countBuffs = 0, balance = 0;
            foreach(var demon in _demons)
            {
                if (demon.IsMainProfit)
                    balance += (demon.Id + 1) * _states.balancePerDemon;
                if (demon.IsAdvProfit)
                    countBuffs++;

                demon.StatesUpdate();
            }

            AddBalance(balance);
            _artefact.Next(countBuffs);
        }

        public void Profit(int hexId)
        {
            if (hexId == CONST.GATE_ID)
                AddCurse(_states.curseProfit + _level * _states.curseProfitPerLevel);
        }

        public void StartTurn()
        {
            foreach (var demon in _demons)
                demon.EffectsUpdate(_states.gateDefense);

            AddCurse(CursePerTurn);
        }

        public void AddBalance(int value)
        {
            _balance.Add(value);

            if (_balance <= _states.minBalance)
                _eventWin.Invoke(Win.Satan);
            if (_balance >= _states.maxBalance)
                _eventWin.Invoke(Win.Human);

            _eventSelf.Invoke(this);
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

            _eventSelf.Invoke(this);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
            _demons.Dispose();
        }

        private void ActorKilling(Id<PlayerId> self, Id<PlayerId> target, int actorId)
        {
            UnityEngine.Debug.Log($"ActorKilling: {self}, {target}, {actorId}");
        }
    }
}
