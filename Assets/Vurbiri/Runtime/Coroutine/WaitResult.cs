using System.Collections;

namespace Vurbiri
{
    public static class WaitResult
    {
        public static WaitResult<T> Instant<T>(T result) => new WaitResultSource<T>(false, result);
    }

    public abstract class WaitResult<T> : Enumerator
    {
        protected bool _isWait = true;
        protected T _value;

        public T Value => _value;

        sealed public override bool MoveNext() => _isWait;

        public static implicit operator T(WaitResult<T> wait) => wait._value;
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
        internal WaitResultSource(bool isWait, T value)
        {
            _isWait = isWait;
            _value = value;
        }

        public WaitResult<T> SetResult(T result)
        {
            _isWait = false;
            _value = result;

            return this;
        }

        public WaitResultSource<T> Recreate()
        {
            _isWait = false;
            _value = _default;

            return new(_default);
        }

        public IEnumerator Restart()
        {
            _isWait = true;
            _value = _default;

            return this;
        }

        public WaitResult<T> Cancel()
        {
            _isWait = false;
            _value = _default;

            return this;
        }

        public new void Reset()
        {
            _isWait = true;
            _value = _default;
        }
    }
}
