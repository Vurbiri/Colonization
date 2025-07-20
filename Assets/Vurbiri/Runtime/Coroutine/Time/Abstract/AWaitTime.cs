using System.Collections;

namespace Vurbiri
{
    [System.Serializable]
    public abstract class AWaitTime : IWait
    {
        [UnityEngine.SerializeField] private float _waitTime;
        private float _waitUntilTime;
        private bool _isRunning;

        protected abstract float ApplicationTime { get; }

        public object Current => null;
        public float Time => _waitTime;
        public float CurrentTime => _waitUntilTime;

        public bool IsRunning => _isRunning;

        public AWaitTime(float time) => _waitTime = time;

        public bool MoveNext()
        {
            if (!_isRunning)
                _waitUntilTime = _waitTime + ApplicationTime;

            return _isRunning = _waitUntilTime > ApplicationTime;
        }

        public IEnumerator Restart(float value)
        {
            _waitTime = value;
            _isRunning = false;
            return this;
        }
        public IEnumerator Restart()
        {
            _isRunning = false;
            return this;
        }

        public void Reset() => _isRunning = false;
    }
}
