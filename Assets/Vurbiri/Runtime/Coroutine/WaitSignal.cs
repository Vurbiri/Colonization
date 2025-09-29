namespace Vurbiri
{
    sealed public class WaitSignal : System.Collections.IEnumerator
    {
        private bool _isWait;

        public object Current => null;
        public bool IsWait => _isWait;

        public WaitSignal() => _isWait = true;
        private WaitSignal(bool isWait) => _isWait = isWait;

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
