namespace Vurbiri
{
    public abstract class WaitResult<T> : System.Collections.IEnumerator
    {
        protected bool _keepWaiting = true;
        protected T _value;

        public object Current => _value;
        public T Value => _value;

        public bool MoveNext() => _keepWaiting;
        public void Reset() { }
    }

    public class WaitResultSource<T> : WaitResult<T>
    {
        public static WaitResultSource<T> Empty { get; } = new(default);

        public bool IsRunning => _keepWaiting;

        public WaitResultSource()
        {
            _keepWaiting = true;
        }
        public WaitResultSource(T result)
        {
            _value = result;
            _keepWaiting = false;
        }

        public WaitResult<T> SetResult(T result)
        {
            _value = result;
            _keepWaiting = false;

            return this;
        }

        public WaitResultSource<T> Recreate()
        {
            _value = default;
            _keepWaiting = false;

            return new();
        }

        public WaitResult<T> Restart()
        {
            _keepWaiting = true;
            _value = default;

            return this;
        }

        public WaitResult<T> Cancel()
        {
            _value = default;
            _keepWaiting = false;

            return this;
        }

        public new void Reset()
        {
            _keepWaiting = true;
            _value = default;
        }
    }
}
