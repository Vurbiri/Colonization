using System.Runtime.CompilerServices;

namespace Vurbiri
{
    [System.Serializable]
    sealed public class WaitTime : AWaitTime
    {
        protected override float ApplicationTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => UnityEngine.Time.time;
        }

        public WaitTime(float time) : base(time) { }

        public static implicit operator WaitTime(float time) => new(time);
    }
}
