//Assets\Vurbiri\Runtime\Coroutine\WaitTime.cs

namespace Vurbiri
{
    public class WaitTime : UnityEngine.CustomYieldInstruction
    {
        private float _waitUntilTime = -1f;
        private float _waitTime;

        public float Time 
        { 
            get => _waitTime; 
            set {  _waitTime = value; _waitUntilTime = -1f; } 
        }

        public override bool keepWaiting
        {
            get
            {
                if (_waitUntilTime < 0f)
                    _waitUntilTime = UnityEngine.Time.time + _waitTime;

                bool flag = UnityEngine.Time.time < _waitUntilTime;
                if (!flag)
                    _waitUntilTime = -1f;

                return flag;
            }
        }

        public WaitTime(float time) => _waitTime = time;

        public WaitTime SetTime(float value)
        {
            Time = value;
            return this;
        }

        public override void Reset() => _waitUntilTime = -1f;
    }
}
