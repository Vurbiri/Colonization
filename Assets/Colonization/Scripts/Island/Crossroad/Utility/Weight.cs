namespace Vurbiri.Colonization
{
    sealed public class Weight
	{
        private int _weight;

        public readonly Key key;

        public Weight(Key key, int weight)
        {
            this.key = key;
            this._weight = weight;
        }

        public Weight Remove(int delta)
        {
            _weight -= delta;
            return this;
        }

        public override string ToString() => _weight.ToString();

        public static int operator +(Weight a, int b) => a._weight + b;
        public static int operator -(Weight a, Weight b) => a._weight - b._weight;

        public static bool operator >(Weight a, int b) => a._weight > b;
        public static bool operator <(Weight a, int b) => a._weight < b;

        public static bool operator >=(Weight a, int b) => a._weight >= b;
        public static bool operator <=(Weight a, int b) => a._weight <= b;
    }
}
