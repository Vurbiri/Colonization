using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Vurbiri
{
    [System.Serializable]
    public abstract class AWaitTime : IEnumerable
    {
        [UnityEngine.SerializeField] private float _waitTime;

        private readonly Timer _timer;

        public float Time => _waitTime;
        public IEnumerator Current => _timer;

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

        public IEnumerator GetEnumerator() => _timer.Set(_waitTime);

        public static implicit operator Enumerator(AWaitTime self) => self._timer.Set(self._waitTime);

        #region Nested Timer
        // *******************************************************
        sealed private class Timer : Enumerator
        {
            private readonly Func<float> _applicationTime;
            private float _waitUntilTime;

            public Timer(Func<float> applicationTime) => _applicationTime = applicationTime;

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
