using System.Collections;

namespace Vurbiri
{
    public class WaitFrames : IWait
    {
        private ushort _waitFrames;
        private ushort _currentFrames;
        private bool _isWait;

        public object Current => null;
        public bool IsWait => _isWait;

        public bool MoveNext()
        {
            if (!_isWait)
                _currentFrames = _waitFrames;

            return _isWait = _currentFrames --> 0;
        }

        public WaitFrames(ushort frames)  => _waitFrames = frames;

        public IEnumerator Restart()
        {
            _isWait = false;
            return this;
        }

        public IEnumerator Restart(ushort value)
        {
            _waitFrames = value;
            _isWait = false;
            return this;
        }

        public void Reset() => _isWait = false;
    }
}
