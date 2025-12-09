using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive
{
	public class ReactiveFlags : ReactiveValue<bool>, System.IDisposable
    {
        public enum Mode { Or, And }

        private readonly Mode _mode;
        private int _flags;
        private int _count;
        private Subscription _subscription;

        public int Count { [Impl(256)] get => _count; }

        [Impl(256)] public ReactiveFlags() => _mode = Mode.Or;
        [Impl(256)] public ReactiveFlags(Mode mode) => _mode = mode;

        public void Add(IReactive<bool> value)
        {
            int index = _count++;
            if(_mode == Mode.Or)
                _subscription += value.Subscribe((flag) => Or(index, flag));
            else
                _subscription += value.Subscribe((flag) => And(index, flag));
        }

        public void Dispose() => _subscription?.Dispose();

        private void Or(int index, bool value)
        {
            _flags = value ? _flags | (1 << index) : _flags & ~(1 << index);
            SetValue(_flags > 0);
        }

        private void And(int index, bool value)
        {
            _flags = value ? _flags & ~(1 << index) : _flags | (1 << index);
            SetValue(_flags == 0);
        }

        [Impl(256)] private void SetValue(bool value)
        {
            if (_value ^ value)
                _onChange.Invoke(_value = value);
        }
    }
}
