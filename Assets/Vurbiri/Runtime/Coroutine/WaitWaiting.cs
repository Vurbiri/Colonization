using System.Collections;

namespace Vurbiri
{
    public class WaitWaiting : IEnumerator
    {
        private IWait _waiting;
        
        public object Current => _waiting;

        public WaitWaiting(IWait waiting) => _waiting = waiting;

        public bool MoveNext() => _waiting.IsRunning;

        public IEnumerator ReSetup(IWait waiting)
        {
            _waiting = waiting;
            return this;
        }

        public void Reset() { }
    }
}
