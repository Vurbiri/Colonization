using System;
using System.Runtime.CompilerServices;

namespace Vurbiri
{
    sealed public class WaitSwitchFloat : AWaitSwitchFloat
    {
        protected override float DeltaTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => UnityEngine.Time.unscaledDeltaTime;
        }

        public WaitSwitchFloat(float min, float max, float speed, Action<float> update) : base(min, max, speed, update) { }
    }
}
