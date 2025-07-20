using System.Collections;

namespace Vurbiri
{
    public static class WaitResult
    {
        public static WaitResult<T> Instant<T>(T result) => new WaitResultSource<T>(result, false);
    }

    public abstract class WaitResult<T> : IWait
    {
        protected bool _keepWaiting = true;
        protected T _value;

        public object Current => _value;
        public T Value => _value;
        public bool IsRunning => _keepWaiting;

        public bool MoveNext() => _keepWaiting;
        public void Reset() { }
    }

    public class WaitResultSource<T> : WaitResult<T>
    {
        private T _default;

        public WaitResultSource()
        {
            _keepWaiting = true;
            _default = default;
        }
        public WaitResultSource(T defaultValue)
        {
            _keepWaiting = true;
            _default = defaultValue;
        }
        internal WaitResultSource(T value, bool plug)
        {
            _keepWaiting = false;
            _value = value;
        }

        public WaitResult<T> SetResult(T result)
        {
            _value = result;
            _keepWaiting = false;

            return this;
        }

        public WaitResultSource<T> Recreate()
        {
            _value = _default;
            _keepWaiting = false;

            return new(_default);
        }

        public IEnumerator Restart()
        {
            _keepWaiting = true;
            _value = _default;

            return this;
        }

        public WaitResult<T> Cancel()
        {
            _keepWaiting = false;
            _value = _default;

            return this;
        }

        public new void Reset()
        {
            _keepWaiting = true;
            _value = _default;
        }
    }
}
