using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public abstract class AWeight
	{
        internal int _weight;

        public int Weight { [Impl(256)] get => _weight; }

        [Impl(256)] protected AWeight(int weight) => _weight = weight;
    }
}
