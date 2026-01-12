namespace Vurbiri
{
	[System.Serializable, Newtonsoft.Json.JsonConverter(typeof(Converter))]
	sealed public class WaitRealtime : AWaitTime
	{
		private static readonly System.Func<float> s_time = typeof(UnityEngine.Time).GetStaticGetor<float>(nameof(UnityEngine.Time.realtimeSinceStartup));

		public WaitRealtime() : base(s_time) { }
		public WaitRealtime(float time) : base(time, s_time) { }
		public WaitRealtime(AWaitTime time) : base(time, s_time) { }


		public static implicit operator WaitRealtime(float time) => new(time);

		// ***** Nested *****
		sealed public class Converter : AConverter
		{
			protected override object TimerCreate(float time) => new WaitRealtime(time);
		}
	}
}
