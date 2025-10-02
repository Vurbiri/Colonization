using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class Breach
	{
        private const int MAX_BREACH = HEX.SIDES * (CONST.MAX_CIRCLES + HEX.SIDES);

        private Weight[] _weights = new Weight[MAX_BREACH];
        private int _count = 1;
        private int _max;

        public int Count { [Impl(256)] get => _count - 1; }

        public Breach() => _weights[0] = new(Key.Zero, 0);

        public void Add(Crossroad crossroad)
        {
            if(crossroad.IsBreach)
            {
                _max = _weights[_count - 1] + crossroad.Weight;
                _weights[_count] = new (crossroad.Key, _max);
                _count++;

                crossroad.ResetedWeight.Add(OnResetedWeight);
            }
        }

        public Key Get()
        {
            return Search(0, _count, UnityEngine.Random.Range(0, _max));
            
            // Local
            Key Search(int min, int max, int weight)
            {
                int current = min + max >> 1;
                var value = _weights[current];
                if (value <= weight)
                     return Search(current, max, weight);
                if (_weights[current - 1] > weight)
                    return Search(min, current, weight);

                return value.key;
            }
        }

        public void TrimExcess()
        {
            Array.Resize(ref _weights, _count);
        }

        private void OnResetedWeight(Key key)
        {
            int index = 1;
            for(; index < _count; index++)
                if(_weights[index].key == key)
                    break;
            
            _count--;
            int delta = _weights[index] - _weights[index - 1];
            for (; index < _count; index++)
                _weights[index] = _weights[index + 1].Remove(delta);
            _max -= delta;
        }
    }
}
