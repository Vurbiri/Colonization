//Assets\Colonization\Scripts\Players\Demon\Demon.cs
using System;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public class Satan : IReactive<Satan>
    {
        private readonly RInt _level;
        private readonly RInt _curse;
        private readonly RInt _balance;
        
        private readonly SatanAbilities _states;

        private readonly DemonBuffs _leveling;
        private readonly Buffs _artefact;

        private readonly DemonsSpawner _spawner;
        private readonly ListReactiveItems<Actor> _demons = new();

        private readonly Subscriber<Satan> _subSelf = new();
        private readonly Subscriber<Win> _subWin = new();
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

        public Satan(SatanSaveData data, Players.Settings settings, Hexagons hexagons, IReadOnlyList<Human> humans)
        {
            _states = settings.satanStates;

            SatanLoadData loadData = data.LoadData;
            bool isLoaded = loadData != null;

            _level = isLoaded ? new(loadData.level) : new();

            _leveling = new(settings.demonBuffs.Settings, _level);
            _artefact = Buffs.Create(settings.artefact.Settings, loadData);

            _spawner = DemonsSpawner.Create(_level, new(_leveling, _artefact), settings, hexagons[Key.Zero], loadData);

            if (isLoaded)
            {
                _curse = new(loadData.curse);
                _balance = new(loadData.balance);

                int count = loadData.actors.Count;
                for (int i = 0; i < count; i++)
                    _demons.Add(_spawner.Load(loadData.actors[i], hexagons));
            }
            else
            {
                _curse = new();
                _balance = new();
            }

            for (int i = 0; i < PlayerId.HumansCount; i++)
            {
                _unsubscribers += humans[i].Shrines.Subscribe(OnShrineBuild, false);
                _unsubscribers += humans[i].Perks.Subscribe(_ => AddBalance(_states.balancePerPerk), false);
            }

            _balance.Subscribe(_ => _subSelf.Invoke(this));

            data.StatusBind(this, !isLoaded);
            data.ArtefactBind(_artefact, !isLoaded);
            data.ActorsBind(_demons);
        }

        public void EndTurn()
        {
            int countBuffs = 0, balance = 0;
            Actor actor;
            for (int i = 0; i < _demons.Count; i++)
            {
                actor = _demons[i];
                if (actor.IsMainProfit)
                    balance += (actor.Id + 1) * _states.balancePerDemon;
                if (actor.IsAdvProfit)
                    countBuffs++;

                actor.StatesUpdate();
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
            for (int i = 0; i < _demons.Count; i++)
                _demons[i].EffectsUpdate(_states.gateDefense);

            AddCurse(CursePerTurn);
        }

        public void AddBalance(int value)
        {
            _balance.Add(value);

            if (_balance <= _states.minBalance)
                _subWin.Invoke(Win.Satan);
            if (_balance >= _states.maxBalance)
                _subWin.Invoke(Win.Human);

            _subSelf.Invoke(this);
        }

        public Unsubscriber Subscribe(Action<Satan> action, bool calling) => _subSelf.Add(action, calling, this);

        public void Dispose()
        {
            _level.Dispose();
            _curse.Dispose();
            _balance.Dispose();
            _unsubscribers.Unsubscribe();
            _subSelf.Dispose();
            _subWin.Dispose();
            _leveling.Dispose();
            _artefact.Dispose();
            _demons.Dispose();
        }

        #region IArrayable
        public const int SIZE_ARRAY = 4;
        public int[] ToArray() => new int[] { _level, _curse, _balance, _spawner.Potential };
        public int[] ToArray(int[] array)
        {
            if(array.Length != SIZE_ARRAY)
                return ToArray();

            int i = 0;
            array[i++] = _level; array[i++] = _curse; array[i++] = _balance; array[i] = _spawner.Potential;
            return array;
        }
        public static void FromArray(IReadOnlyList<int> array, out int level, out int curse, out int balance, out int spawn)
        {
            Errors.ThrowIfLengthNotEqual(array, SIZE_ARRAY);

            int i = 0;
            level = array[i++];
            curse = array[i++];
            balance = array[i++];
            spawn = array[i];
        }
        #endregion

        private void AddCurse(int value)
        {
            _curse.Add(value);

            int maxCurse = MaxCurse;
            if (_curse >= maxCurse)
            {
                _curse.Remove(maxCurse);
                _level.Increment();
            }

            _subSelf.Invoke(this);
        }

        private void OnShrineBuild(int index, Crossroad crossroad, TypeEvent type)
        {
            if (type == TypeEvent.Add)
                AddBalance(_states.balancePerShrine);
        }
    }
}
