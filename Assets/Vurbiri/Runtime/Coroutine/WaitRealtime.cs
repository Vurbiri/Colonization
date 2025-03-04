//Assets\Vurbiri\Runtime\Coroutine\WaitRealtime.cs
namespace Vurbiri
{
    public class WaitRealtime : AWaitTime
    {
        protected override float ApplicationTime => UnityEngine.Time.realtimeSinceStartup;

        public WaitRealtime(float time) : base(time) { }
    }
}
