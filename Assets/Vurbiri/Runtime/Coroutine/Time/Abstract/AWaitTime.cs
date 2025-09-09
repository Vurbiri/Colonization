using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Vurbiri
{
    [System.Serializable]
    public abstract class AWaitTime : Enumerator
    {
        [UnityEngine.SerializeField] private float _waitTime;

        private readonly Timer _timer;
        private bool _isWait;

        public float Time => _waitTime;
        public IEnumerator CurrentTimer => _timer;

        protected AWaitTime(Func<float> applicationTime) => _timer = new(applicationTime);
        protected AWaitTime(float time, Func<float> applicationTime) : this(applicationTime) => _waitTime = time;
        protected AWaitTime(AWaitTime time, Func<float> applicationTime) : this(applicationTime) => _waitTime = time._waitTime;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator Restart() => _timer.Set(_waitTime);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator Restart(float value)
        {
            _waitTime = value;
            return _timer.Set(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator OffsetRestart(float offset) => _timer.Set(_waitTime + offset);

        sealed public override bool MoveNext()
        {
            if (!_isWait)
                _timer.Set(_waitTime);

            return _isWait = _timer.MoveNext();
        }

        #region Nested Timer
        // *******************************************************
        sealed private class Timer : Enumerator
        {
            private readonly Func<float> _applicationTime;
            private float _waitUntilTime;

            public Timer(Func<float> applicationTime) => _applicationTime = applicationTime;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override bool MoveNext() => _waitUntilTime > _applicationTime();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator Set(float time)
            {
                _waitUntilTime = time + _applicationTime();
                return this;
            }
        }
        #endregion
    }
}
