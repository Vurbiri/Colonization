namespace Vurbiri
{
    sealed public class WaitSignal : System.Collections.IEnumerator
    {
        private bool _keepWaiting;

        public object Current => null;

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
