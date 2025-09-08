using System.Collections;

namespace Vurbiri
{
    sealed public class WaitFrames : Enumerator
    {
        private ushort _waitFrames;
        private ushort _currentFrames;
        private bool _isWait;

        public bool IsWait => _isWait;

        public override bool MoveNext()
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
    }
}
