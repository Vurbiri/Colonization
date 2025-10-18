namespace Vurbiri
{
    [System.Serializable, Newtonsoft.Json.JsonConverter(typeof(Converter))]
    sealed public class WaitScaledTime : AWaitTime
    {
        private static readonly System.Func<float> s_time = typeof(UnityEngine.Time).GetStaticGetor<float>(nameof(UnityEngine.Time.time));

        public WaitScaledTime() : base(s_time) { }
        public WaitScaledTime(float time) : base(time, s_time) { }
        public WaitScaledTime(AWaitTime time) : base(time, s_time) { }

        public static implicit operator WaitScaledTime(float time) => new(time);


        // ***** Nested *****
        sealed public class Converter : AConverter
        {
            protected override object TimerCreate(float time) => new WaitScaledTime(time);
        }
    }
}
