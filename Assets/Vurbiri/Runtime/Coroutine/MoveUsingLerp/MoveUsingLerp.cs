using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri
{
    [Serializable]
    sealed public class MoveUsingLerp : AMoveUsingLerp
    {
        public MoveUsingLerp(Transform transform, float speed) : base(transform, speed)
        {
        }

        protected override float DeltaTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => UnityEngine.Time.unscaledDeltaTime;
        }
    }
}
