using Newtonsoft.Json;
using System;
using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

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

        [Impl(256)] public IEnumerator Restart() => _timer.Set(_waitTime);

        [Impl(256)] public IEnumerator Restart(float value)
        {
            _waitTime = value;
            return _timer.Set(value);
        }

        [Impl(256)] public IEnumerator OffsetRestart(float offset) => _timer.Set(_waitTime + offset);

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

            [Impl(256)] public override bool MoveNext() => _waitUntilTime > _applicationTime();

            [Impl(256)] public Enumerator Set(float time)
            {
                _waitUntilTime = time + _applicationTime();
                return this;
            }
        }
        // *******************************************************
        public abstract class AConverter : AJsonConverter<AWaitTime>
        {
            sealed public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return TimerCreate(serializer.Deserialize<float>(reader));
            }

            protected abstract object TimerCreate(float time);

            sealed protected override void WriteJson(JsonWriter writer, AWaitTime value, JsonSerializer serializer)
            {
                writer.WriteValue(value._waitTime);
            }
        }
        // *******************************************************
        #endregion
    }
}
