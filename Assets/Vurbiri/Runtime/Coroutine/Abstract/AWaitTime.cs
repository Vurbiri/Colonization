//Assets\Vurbiri\Runtime\Coroutine\Abstract\AWaitTime.cs
namespace Vurbiri
{
    [System.Serializable]
    public abstract class AWaitTime : UnityEngine.CustomYieldInstruction
    {
        [UnityEngine.SerializeField] private float _waitTime;
        private float _waitUntilTime = -1f;

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

        public AWaitTime Restart(float value)
        {
            _waitTime = value;
            _waitUntilTime = ApplicationTime + _waitTime;
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
