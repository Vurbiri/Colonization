//Assets\Colonization\Scripts\Island\Hexagon\Hexagons.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Hexagons : IReactive<Hexagon>, IDisposable
    {
        private readonly Dictionary<Key, Hexagon> _hexagons = new(MAX_HEXAGONS);
        private readonly Dictionary<int, List<Key>> _hexagonsIdForKey;
        private readonly Subscription<Hexagon> _eventChanged = new();

        public Hexagon this[Key key] => _hexagons[key];
        public static CurrenciesLite FreeResources { get; private set; }

        public Hexagons(GameEvents events)
        {
            int count = HEX_IDS.Length, capacity = MAX_HEXAGONS / count + 1;
            _hexagonsIdForKey = new(count + 1);

            for (int i = 0; i < count; i++)
                _hexagonsIdForKey[HEX_IDS[i]] = new List<Key>(capacity);
            _hexagonsIdForKey[GATE_ID] = new List<Key>(1);

            events.Subscribe(GameModeId.EndTurn, OnEndTurn);
            events.Subscribe(GameModeId.Roll, OnRoll);
        }

        public Hexagon Add(Key key, int id, Hexagon hex)
        {
            _eventChanged.Invoke(hex);

            _hexagons.Add(key, hex);
            _hexagonsIdForKey[id].Add(key);

            return hex;
        }

        public Unsubscription Subscribe(Action<Hexagon> action, bool instantGetValue = true) => _eventChanged.Add(action);

        public void Dispose()
        {
            FreeResources = null;
        }

        private void OnEndTurn(TurnQueue turnQueue, int id)
        {
            if (id > 0)
            {
                foreach (var key in _hexagonsIdForKey[id])
                    _hexagons[key].ResetProfit();
            }
        }
        private void OnRoll(TurnQueue turnQueue, int id)
        {
            CurrenciesLite freeResources = new();
            foreach (var key in _hexagonsIdForKey[id])
                if (_hexagons[key].SetAndGetFreeProfit(out int currencyId))
                    freeResources.Increment(currencyId);

            FreeResources = freeResources;
        }


        public static implicit operator Dictionary<Key, Hexagon>(Hexagons hexagons) => hexagons._hexagons;
    }
}
