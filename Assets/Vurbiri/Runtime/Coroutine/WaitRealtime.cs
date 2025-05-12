//Assets\Vurbiri\Runtime\Coroutine\WaitRealtime.cs
using System.Runtime.CompilerServices;

namespace Vurbiri
{
    [System.Serializable]
    sealed public class WaitRealtime : AWaitTime
    {
        protected override float ApplicationTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => UnityEngine.Time.realtimeSinceStartup;
        }

        public WaitRealtime(float time) : base(time) { }

        public static implicit operator WaitRealtime(float time) => new(time);
    }
}
