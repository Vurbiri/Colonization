using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    [JsonArray]
    public class RoadData : IEnumerable<int>
    {
        private readonly HashSet<Key> _roads;

        public IEnumerable<Key> Roads => _roads;

        public RoadData()
        {
            _roads = new();
        }

        [JsonConstructor]
        public RoadData(int[] array)
        {
            int count = array.Length;
            if (count < 2 || count % 2 != 0)
                throw new Exception($"RoadData: неверный размер входного массива ({array.Length})");
            
            _roads = new(count >> 1);
            for (int i = 0; i < count; i++)
                _roads.Add(new(array[i++], array[i]));
        }

        public void Add(Key key)
        {
            _roads.Add(key);
        }

        public IEnumerator<int> GetEnumerator()
        {
            foreach(Key key in _roads)
            {
                yield return key.X;
                yield return key.Y;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
