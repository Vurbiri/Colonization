using System;
using System.Collections.Generic;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Hexagons : IReactive<Hexagon>, IDisposable
    {
        private readonly Dictionary<Key, Hexagon> _hexagons = new(MAX_HEXAGONS);
        private readonly List<Key>[] _hexagonsIdForKey = new List<Key>[HEX_IDS[^1] + 1];
        private readonly Subscription<Hexagon> _eventChanged = new();
        private readonly CurrenciesLite _freeResources = new();

        private int _groundCount = 0;

        public Hexagon this[Key key] => _hexagons[key];
        public CurrenciesLite FreeResources => _freeResources;
        public int GroundCount => _groundCount;

        public Hexagons(Pool<HexagonMark> poolMarks)
        {
            int count = HEX_IDS.Length, capacity = MAX_HEXAGONS / count + 1;

            for (int i = 0; i < count; i++)
                _hexagonsIdForKey[HEX_IDS[i]] = new List<Key>(capacity);
            _hexagonsIdForKey[GATE_ID] = new List<Key>(1);

            GameContainer.GameEvents.Subscribe(GameModeId.EndTurn, OnEndTurn);
            GameContainer.GameEvents.Subscribe(GameModeId.Roll, OnRoll);

            Hexagon.Init(poolMarks);
        }

        public Hexagon Add(Key key, int id, Hexagon hex)
        {
            _eventChanged.Invoke(hex);

            _hexagons.Add(key, hex);
            _hexagonsIdForKey[id].Add(key);

            if (hex.IsGround) _groundCount++;

            return hex;
        }

        public void SwapId(Hexagon hexA, Hexagon hexB, UnityEngine.Color color, float showTime)
        {
            int idA = hexA.ID, idB = hexB.ID;

            hexA.NewId(idB, color, showTime); hexB.NewId(idA, color, showTime);

            var keys = _hexagonsIdForKey[idA];
            keys.Remove(hexA.Key); keys.Add(hexB.Key);
            keys = _hexagonsIdForKey[idB];
            keys.Remove(hexB.Key); keys.Add(hexA.Key);

            Banner.Open($"{idA:D2} <-> {idB:D2}", color, showTime);

            _eventChanged.Invoke(hexA); _eventChanged.Invoke(hexB);
        }

        public Unsubscription Subscribe(Action<Hexagon> action, bool instantGetValue = true) => _eventChanged.Add(action);

        public void Dispose()
        {
            Hexagon.Clear();
        }

        private void OnEndTurn(TurnQueue turnQueue, int id)
        {
            if (id > 0)
            {
                var keys = _hexagonsIdForKey[id];
                for(int i = keys.Count - 1; i >= 0; i--)
                    _hexagons[keys[i]].ResetProfit();
            }
        }
        private void OnRoll(TurnQueue turnQueue, int id)
        {
            _freeResources.Clear();
            var keys = _hexagonsIdForKey[id];
            for (int i = keys.Count - 1; i >= 0; i--)
            {
                if (_hexagons[keys[i]].SetProfitAndTryGetFreeProfit(out int currencyId))
                    _freeResources.IncrementMain(currencyId);
            }
        }

        public static implicit operator Dictionary<Key, Hexagon>(Hexagons hexagons) => hexagons._hexagons;
    }
}
