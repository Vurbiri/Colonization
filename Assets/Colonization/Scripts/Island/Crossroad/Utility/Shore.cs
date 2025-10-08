using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class Shore
	{
        private Weight[] _weights = new Weight[HEX.SIDES * (CONST.MAX_CIRCLES + HEX.SIDES)];
        private int _max, _count = 1;
        
        public int Count { [Impl(256)] get => _count - 1; }

        public Shore()
        {
            _weights[0] = new(Key.Zero, 0);
        }

        public void Add(Crossroad crossroad)
        {
            if(crossroad.CanBuildOnShore)
            {
                _max = _weights[_count - 1] + crossroad.Weight;
                _weights[_count] = new (crossroad.Key, _max);
                _count++;

                crossroad.BannedBuild.Add(OnBannedBuild);
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

        private void OnBannedBuild(Key key)
        {
            int index = 0;
            while (_weights[++index].key != key);

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

        #region Nested Weight
        //**********************************************************
        sealed private class Weight
        {
            private int _weight;

            public readonly Key key;

            [Impl(256)] public Weight(Key key, int weight)
            {
                this.key = key;
                this._weight = weight;
            }

            [Impl(256)] public Weight Add(int delta)
            {
                _weight += delta;
                return this;
            }

            [Impl(256)] public Weight Remove(int delta)
            {
                _weight -= delta;
                return this;
            }

            public override string ToString() => _weight.ToString();


            public static implicit operator int(Weight self) => self._weight;

            public static int operator +(Weight w1, Weight w2) => w1._weight + w2._weight;
            public static int operator +(Weight w, int i) => w._weight + i;
            public static int operator -(Weight w1, Weight w2) => w1._weight - w2._weight;
            public static int operator -(Weight w, int i) => w._weight - i;

            public static bool operator >(Weight w, int i) => w._weight > i;
            public static bool operator <(Weight w, int i) => w._weight < i;
            public static bool operator >=(Weight w, int i) => w._weight >= i;
            public static bool operator <=(Weight w, int i) => w._weight <= i;

            public static bool operator >(int i, Weight w) => i > w._weight;
            public static bool operator <(int i, Weight w) => i < w._weight;
            public static bool operator >=(int i, Weight w) => i >= w._weight;
            public static bool operator <=(int i, Weight w) => i <= w._weight;
        }
        #endregion
    }
}
