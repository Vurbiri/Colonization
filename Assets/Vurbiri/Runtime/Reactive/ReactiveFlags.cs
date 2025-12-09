using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive
{
	public class ReactiveFlags : ReactiveValue<bool>
	{
        private const int BASE_CAPACITY = 3;

        private bool[] _flags;
		private int _count, _capacity;

        public int Count { [Impl(256)] get => _count; }

    }
}
