//Assets\Vurbiri\Runtime\Coroutine\Abstract\AWaitTime.cs
namespace Vurbiri
{
    public abstract class AWaitTime : UnityEngine.CustomYieldInstruction
    {
        private float _waitUntilTime = -1f;
        private float _waitTime;

        protected abstract float ApplicationTime { get; }

        public float Time
        {
            get => _waitTime;
            set { _waitTime = value; _waitUntilTime = -1f; }
        }

        public override bool keepWaiting
        {
            get
            {
                if (_waitUntilTime < 0f)
                    _waitUntilTime = ApplicationTime + _waitTime;

                bool flag = ApplicationTime < _waitUntilTime;
                if (!flag)
                    _waitUntilTime = -1f;

                return flag;
            }
        }

        public AWaitTime(float time) => _waitTime = time;

        public AWaitTime SetTime(float value)
        {
            Time = value;
            return this;
        }

        public AWaitTime Restart()
        {
            _waitUntilTime = ApplicationTime + _waitTime;
            return this;
        }

        public override void Reset() => _waitUntilTime = -1f;
    }
}
