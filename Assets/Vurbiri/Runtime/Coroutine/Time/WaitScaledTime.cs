using System.Runtime.CompilerServices;

namespace Vurbiri
{
    [System.Serializable]
    sealed public class WaitScaledTime : AWaitTime
    {
        protected override float ApplicationTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => UnityEngine.Time.time;
        }

        public WaitScaledTime(float time) : base(time) { }

        public static implicit operator WaitScaledTime(float time) => new(time);
    }
}
