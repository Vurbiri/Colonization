using System.Collections;
using System.Runtime.CompilerServices;

namespace Vurbiri
{
    [System.Serializable]
    public abstract class AWaitTime : IWait
    {
        [UnityEngine.SerializeField] private float _waitTime;
        private float _deltaTime;
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
                _waitUntilTime = _waitTime + _deltaTime + ApplicationTime;

            return _isWait = _waitUntilTime > ApplicationTime;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator Restart(float value) => Restart(value, 0f);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator Restart(float value, float delta)
        {
            _waitTime = value;
            _deltaTime = delta;
            _isWait = false;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator Restart()
        {
            _deltaTime = 0f;
            _isWait = false;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator RestartUsingDelta(float delta)
        {
            _deltaTime = delta;
            _isWait = false;
            return this;
        }

        public void Reset() => _isWait = false;
    }
}
