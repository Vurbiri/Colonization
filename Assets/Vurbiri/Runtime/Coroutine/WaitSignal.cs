namespace Vurbiri
{
    sealed public class WaitSignal : System.Collections.IEnumerator
    {
        private bool _isWait;

        public object Current => null;
        public bool IsWait => _isWait;

        public WaitSignal() => _isWait = true;

        public bool MoveNext() => _isWait;
        public void Send() => _isWait = false;
        public void Reset() => _isWait = true;
        public WaitSignal Restart()
        {
            _isWait = true;
            return this;
        }
    }
}
