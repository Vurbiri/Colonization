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
    public class Satan : IReactive<IArrayable>, IArrayable
    {
        private readonly RInt _level;
        private readonly RInt _curse;
        private int _cursePerTurn;
        
        private readonly SatanAbilities _states;

        private readonly DemonBuffs _leveling;
        private readonly Buffs _artefact;

        private readonly Hexagon _gateHex;
        private readonly DemonsSpawner _spawner;
        private readonly ListReactiveItems<Actor> _demons = new();

        private readonly Subscriber<IArrayable> _subscriber = new();
        private Unsubscribers _unsubscribers = new();

        public int MaxCurse => _states.maxCurse + _level * _states.maxCursePerLevel;

        public Satan(SatanSaveData data, Players.Settings settings, Hexagons land)
        {
            _gateHex = land[Key.Zero];
            _states = settings.satanStates;

            SatanLoadData loadData = data.LoadData;
            bool isLoaded = loadData != null;
            if (isLoaded)
            {
                _level = new(loadData.level);
                _curse = new(loadData.curse);

                _leveling = new(settings.demonBuffs.Settings, _level);
                _artefact = new(settings.artefact.Settings, loadData.artefact);

                _spawner = new(loadData.spawnPotential, _level, new(_leveling, _artefact), settings, _gateHex);

                int count = loadData.actors.Count;
                for (int i = 0; i < count; i++)
                    _demons.Add(_spawner.Load(loadData.actors[i], land));
            }
            else
            {
                _level = new();
                _curse = new();

                _leveling = new(settings.demonBuffs.Settings);
                _artefact = new(settings.artefact.Settings);

                _spawner = new(_level, new(_leveling, _artefact), settings, _gateHex);
            }

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
                    balance++;
                if (actor.IsAdvProfit)
                    countBuffs++;

                actor.StatesUpdate();
            }


            _artefact.Next(countBuffs);
        }

        public void Profit(int hexId)
        {
            if (hexId == CONST.GATE_ID)
                CurseAdd(_states.curseProfit + _level * _states.curseProfitPerLevel);
        }

        public void StartTurn()
        {
            CurseAdd(_cursePerTurn);
        }

        public void SetBalance(IReactiveValue<int> value) => _unsubscribers += value.Subscribe(OnBalance);


        public Unsubscriber Subscribe(Action<IArrayable> action, bool calling) => _subscriber.Add(action, calling, this);

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
            _subscriber.Dispose();
            _leveling.Dispose();
            _artefact.Dispose();
            _demons.Dispose();
        }

        #region IArrayable
        public const int SIZE_ARRAY = 3;
        public int[] ToArray() => new int[] { _level, _curse, _spawner.Potential };
        public int[] ToArray(int[] array)
        {
            if(array.Length != SIZE_ARRAY)
                return ToArray();

            int i = 0;
            array[i++] = _level; array[i++] = _curse; array[i] = _spawner.Potential;
            return array;
        }
        public static void FromArray(IReadOnlyList<int> array, out int level, out int curse, out int spawn)
        {
            Errors.ThrowIfLengthNotEqual(array, SIZE_ARRAY);

            int i = 0;
            level = array[i++];
            curse = array[i++];
            spawn = array[i];
        }
        #endregion

        private void CurseAdd(int value)
        {
            _curse.Add(value);

            int maxCurse = MaxCurse;
            if (_curse >= maxCurse)
            {
                _curse.Remove(maxCurse);
                _level.Increment();
            }

            _subscriber.Invoke(this);
        }

        private void OnBalance(int value)
        {
            int ratio = (value > 0 ? _states.ratioPenaltyCurse : _states.ratioRewardCurse) >> SatanAbilities.SHIFT_RATIO;
            _cursePerTurn = _states.cursePerTurn - value * ratio;
        }
    }
}
