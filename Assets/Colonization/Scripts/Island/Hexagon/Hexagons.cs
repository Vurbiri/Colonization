//Assets\Colonization\Scripts\Island\Hexagon\Hexagons.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Hexagons : IReactive<Hexagon>
    {
        private readonly Dictionary<Key, Hexagon> _hexagons = new(MAX_HEXAGONS);
        private readonly Dictionary<int, List<Key>> _hexagonsIdForKey;
        private readonly Signer<Hexagon> _signer = new();

        public Hexagon this[Key key] => _hexagons[key];

        public Hexagons()
        {
            int count = HEX_IDS.Length, capacity = MAX_HEXAGONS / count + 1;
            _hexagonsIdForKey = new(count + 1);

            for (int i = 0; i < count; i++)
                _hexagonsIdForKey[HEX_IDS[i]] = new List<Key>(capacity);
            _hexagonsIdForKey[GATE_ID] = new List<Key>(1);
        }

        public Hexagon Add(Key key, int id, Hexagon hex)
        {
            _signer.Invoke(hex);

            _hexagons.Add(key, hex);
            _hexagonsIdForKey[id].Add(key);

            return hex;
        }

        public CurrenciesLite Profit(int prevId, int currId)
        {
            if (prevId > 0)
            {
                foreach (var key in _hexagonsIdForKey[prevId])
                    _hexagons[key].ResetProfit();
            }

            CurrenciesLite res = new();
            foreach (var key in _hexagonsIdForKey[currId])
                if (_hexagons[key].SetAndGetFreeProfit(out int currencyId))
                    res.Increment(currencyId);

            return res;
        }

        public Unsubscriber Subscribe(Action<Hexagon> action, bool instantGetValue = true) => _signer.Add(action);

        public static implicit operator Dictionary<Key, Hexagon>(Hexagons hexagons) => hexagons._hexagons;
    }
}
