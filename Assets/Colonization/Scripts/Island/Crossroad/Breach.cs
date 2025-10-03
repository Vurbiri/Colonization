using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class Breach
	{
        private Weight[] _weights = new Weight[HEX.SIDES * (CONST.MAX_CIRCLES + HEX.SIDES)];
        private readonly int _min;
        private int _max, _count = 1;
        
        public int Count { [Impl(256)] get => _count - 1; }

        [Impl(256)] public Breach() => _weights[0] = new(Key.Zero, 0);
        [Impl(256)] public Breach(int min) : this() => _min = min;

        public void Add(Crossroad crossroad)
        {
            if(crossroad.IsBreach & crossroad.Weight > _min)
            {
                _max = _weights[_count - 1] + crossroad.Weight;
                _weights[_count] = new (crossroad.Key, _max);
                _count++;

                crossroad.ResetedWeight.Add(OnResetedWeight);
            }
        }

        public Key Get()
        {
            int weight = UnityEngine.Random.Range(0, _max);
            int min = 0, max = _count, current;
            while (true)
            {
                current = min + max >> 1;
                if (_weights[current] <= weight)
                    min = current;
                else if (_weights[current - 1] > weight)
                    max = current;
                else
                    return _weights[current].key;
            }
        }

        [Impl(256)] public void TrimExcess()
        {
            System.Array.Resize(ref _weights, _count); 
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

        private Key Search(int min, int max, int weight)
        {
            int current = min + max >> 1;

            if (_weights[current] <= weight)
                return Search(current, max, weight);
            if (_weights[current - 1] > weight)
                return Search(min, current, weight);

            //UnityEngine.Debug.Log($"{weight} - [{_weights[current - 1]}; {_weights[current]})");
            return _weights[current].key;
        }
    }
}
