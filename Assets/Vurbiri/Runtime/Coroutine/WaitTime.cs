//Assets\Vurbiri\Runtime\Coroutine\WaitTime.cs
using System.Runtime.CompilerServices;

namespace Vurbiri
{
    sealed public class WaitTime : AWaitTime
    {
        protected override float ApplicationTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => UnityEngine.Time.time;
        }

        public WaitTime(float time) : base(time) { }
    }
}
