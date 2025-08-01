using System;

namespace Vurbiri
{
    public class WaitState<T> : IWait where T : Enum
    {
        private readonly WaitStateSource<T> _source;
        private readonly T _waitState;
        private bool _isWait;

        internal WaitState(WaitStateSource<T> source, T waitValue)
        {
            _source = source;
            _waitState = waitValue;
        }

        public T CurrentState => _source._state;
        public object Current => null;
        public bool IsWait => _isWait;

        public bool MoveNext() => _isWait = _waitState.GetHashCode() != _source._state.GetHashCode();
        public void Reset() { }
    }

    public abstract class WaitStateSource<T> where T : Enum
    {
        internal T _state;

        public WaitState<T> SetWaitState(T value) => new(this, value);
    }

    public class WaitStateController<T> : WaitStateSource<T> where T : Enum
    {
        public WaitStateController() => _state = default;
        public WaitStateController(T defaultValue) => _state = defaultValue;

        public void SetState(T value) => _state = value;
        public void Reset() => _state = default;
    }
}
