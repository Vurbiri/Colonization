//Assets\Vurbiri\Runtime\Coroutine\WaitTime.cs

namespace Vurbiri
{
    sealed public class WaitTime : AWaitTime
    {
        protected override float ApplicationTime => UnityEngine.Time.time;

        public WaitTime(float time) : base(time) { }
    }
}
