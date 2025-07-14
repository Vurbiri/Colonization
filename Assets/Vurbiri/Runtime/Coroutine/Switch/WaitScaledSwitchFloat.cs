using System;
using System.Runtime.CompilerServices;

namespace Vurbiri
{
    [Serializable]
    sealed public class WaitScaledSwitchFloat : AWaitSwitchFloat
    {
        protected override float DeltaTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => UnityEngine.Time.deltaTime;
        }

        public WaitScaledSwitchFloat(float min, float max, float speed, Action<float> update) : base(min, max, speed, update) { }
    }
}
