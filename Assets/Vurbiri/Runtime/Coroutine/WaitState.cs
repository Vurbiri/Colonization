using System;

namespace Vurbiri
{
    public class WaitState<T> : Enumerator where T : Enum
    {
        private readonly WaitStateSource<T> _source;
        private readonly int _waitStateHashCode;

        internal WaitState(WaitStateSource<T> source, T waitValue)
        {
            _source = source;
            _waitStateHashCode = waitValue.GetHashCode();
        }

        sealed public override bool MoveNext() => _waitStateHashCode != _source._stateHashCode;
    }

    public abstract class WaitStateSource<T> where T : Enum
    {
        internal int _stateHashCode;

        public WaitState<T> SetWaitState(T value) => new(this, value);
    }

    public class WaitStateController<T> : WaitStateSource<T> where T : Enum
    {
        public WaitStateController() => _stateHashCode = default(T).GetHashCode();
        public WaitStateController(T defaultValue) => _stateHashCode = defaultValue.GetHashCode();

        public void SetState(T value) => _stateHashCode = value.GetHashCode();
        public void Reset() => _stateHashCode = default(T).GetHashCode();
    }
}
