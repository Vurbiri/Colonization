using System.Collections;

namespace Vurbiri
{
    [System.Serializable]
    public abstract class AWaitTime : IWait
    {
        [UnityEngine.SerializeField] private float _waitTime;
        private float _waitUntilTime;
        private bool _isWait;

        protected abstract float ApplicationTime { get; }

        public object Current => null;
        public float Time => _waitTime;
        public float CurrentTime => _waitUntilTime;

        public bool IsWait => _isWait;

        public AWaitTime(float time) => _waitTime = time;

        public bool MoveNext()
        {
            if (!_isWait)
                _waitUntilTime = _waitTime + ApplicationTime;

            return _isWait = _waitUntilTime > ApplicationTime;
        }

        public IEnumerator Restart(float value)
        {
            _waitTime = value;
            _isWait = false;
            return this;
        }
        public IEnumerator Restart()
        {
            _isWait = false;
            return this;
        }

        public void Reset() => _isWait = false;
    }
}
