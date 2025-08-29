using System.Collections;
using System.Runtime.CompilerServices;

namespace Vurbiri
{
    [System.Serializable]
    public abstract class AWaitTime : IWait
    {
        [UnityEngine.SerializeField] private float _waitTime;
        private float _offsetTime;
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
                _waitUntilTime = _waitTime + _offsetTime + ApplicationTime;

            return _isWait = _waitUntilTime > ApplicationTime;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator Restart(float value) => OffsetRestart(value, 0f);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator OffsetRestart(float value, float offset)
        {
            _waitTime = value;
            _offsetTime = offset;
            _isWait = false;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator Restart()
        {
            _offsetTime = 0f;
            _isWait = false;
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator OffsetRestart(float offset)
        {
            _offsetTime = offset;
            _isWait = false;
            return this;
        }

        public void Reset() => _isWait = false;
    }
}
