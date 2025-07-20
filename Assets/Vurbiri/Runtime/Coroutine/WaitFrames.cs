using System.Collections;

namespace Vurbiri
{
    public class WaitFrames : IWait
    {
        private ushort _waitFrames;
        private ushort _currentFrames;
        private bool _isRunning;

        public object Current => null;
        public bool IsRunning => _isRunning;

        public bool MoveNext()
        {
            if (!_isRunning)
                _currentFrames = _waitFrames;

            return _isRunning = _currentFrames --> 0;
        }

        public WaitFrames(ushort frames)  => _waitFrames = frames;

        public IEnumerator Restart()
        {
            _isRunning = false;
            return this;
        }

        public IEnumerator Restart(ushort value)
        {
            _waitFrames = value;
            _isRunning = false;
            return this;
        }

        public void Reset() => _isRunning = false;
    }
}
