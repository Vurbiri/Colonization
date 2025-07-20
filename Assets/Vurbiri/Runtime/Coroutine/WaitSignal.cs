namespace Vurbiri
{
    sealed public class WaitSignal : IWait
    {
        private bool _keepWaiting;

        public object Current => null;
        public bool IsRunning => _keepWaiting;

        public WaitSignal() => _keepWaiting = true;

        public bool MoveNext() => _keepWaiting;
        public void Send() => _keepWaiting = false;
        public void Reset() => _keepWaiting = true;
        public WaitSignal Restart()
        {
            _keepWaiting = true;
            return this;
        }
    }
}
