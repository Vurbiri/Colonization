using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public static class WaitResult
    {
        [Impl(256)] public static WaitResult<T> Instant<T>(T result) => new WaitResultSource<T>(false, result);
    }

    public abstract class WaitResult<T> : Enumerator
    {
        protected bool _isWait = true;
        protected T _value;

        public bool IsWait { [Impl(256)] get => _isWait; }

        public T Value { [Impl(256)] get => _value; }
        public bool IsNull { [Impl(256)] get => _value == null; }
        public bool IsNotNull { [Impl(256)] get => _value != null; }

        sealed public override bool MoveNext() => _isWait;

        [Impl(256)] public static implicit operator T(WaitResult<T> wait) => wait._value;
    }

    sealed public class WaitResultSource<T> : WaitResult<T>
    {
        private readonly T _default;

        public WaitResultSource()
        {
            _isWait = true;
            _default = default;
        }
        public WaitResultSource(T defaultValue)
        {
            _isWait = true;
            _default = defaultValue;
        }
        public WaitResultSource(bool isWait)
        {
            _isWait = isWait;
            _default = _value = default;
        }
        public WaitResultSource(bool isWait, T value)
        {
            _isWait = isWait;
            _default = _value = value;
        }

        [Impl(256)] public WaitResult<T> SetResult(T result)
        {
            _isWait = false;
            _value = result;

            return this;
        }

        [Impl(256)] public WaitResultSource<T> Recreate()
        {
            _isWait = false;
            _value = _default;

            return new(_default);
        }

        [Impl(256)] public WaitResult<T> Restart()
        {
            _isWait = true;
            _value = _default;

            return this;
        }

        [Impl(256)] public WaitResult<T> Cancel()
        {
            _isWait = false;
            _value = _default;

            return this;
        }

        [Impl(256)] public new void Reset()
        {
            _isWait = true;
            _value = _default;
        }
    }
}
