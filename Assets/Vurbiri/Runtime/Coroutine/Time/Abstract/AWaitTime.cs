using System.Collections;

namespace Vurbiri
{
    [System.Serializable]
    public abstract class AWaitTime : IEnumerator
    {
        [UnityEngine.SerializeField] private float _waitTime;
        private float _waitUntilTime = -1f;

        public object Current => null;
        protected abstract float ApplicationTime { get; }
        public float Time
        {
            get => _waitTime;
            set { _waitTime = value; _waitUntilTime = -1f; }
        }

        public AWaitTime(float time) => _waitTime = time;

        public bool MoveNext()
        {
            if (_waitUntilTime < 0f)
                _waitUntilTime = ApplicationTime + _waitTime;

            bool flag = ApplicationTime < _waitUntilTime;
            if (!flag)
                _waitUntilTime = -1f;

            return flag;
        }

        public void Add(float value) => _waitTime += value;
        public void Multiply(float value) => _waitTime *= value;

        public IEnumerator Restart(float value)
        {
            _waitTime = value;
            _waitUntilTime = ApplicationTime + _waitTime;
            return this;
        }
        public IEnumerator Restart()
        {
            _waitUntilTime = ApplicationTime + _waitTime;
            return this;
        }

        public void Reset() => _waitUntilTime = -1f;
    }
}
