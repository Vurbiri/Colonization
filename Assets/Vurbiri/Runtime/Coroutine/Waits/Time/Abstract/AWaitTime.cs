using Newtonsoft.Json;
using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable]
	public abstract class AWaitTime : AWait
	{
		[UnityEngine.SerializeField] private float _waitTime;

		private readonly Timer _timer;

		sealed public override bool IsWait { [Impl(256)] get => _timer.IsWait; }
		public float Time { [Impl(256)] get => _waitTime; [Impl(256)] set => _waitTime = value; } 
		public AWait CurrentTimer { [Impl(256)] get => _timer; }

		[Impl(256)] protected AWaitTime(Func<float> applicationTime) => _timer = new(applicationTime);
		[Impl(256)] protected AWaitTime(float time, Func<float> applicationTime) : this(applicationTime) => _waitTime = time;
		[Impl(256)] protected AWaitTime(AWaitTime time, Func<float> applicationTime) : this(applicationTime) => _waitTime = time._waitTime;

		sealed public override bool MoveNext()
		{
			if (!_timer.IsWait)
				_timer.Set(_waitTime);

			return _timer.MoveNext();
		}
		
		[Impl(256)] public AWait Restart() => _timer.Set(_waitTime);
		[Impl(256)] public AWait Restart(float value)
		{
			_waitTime = value;
			return _timer.Set(value);
		}
		[Impl(256)] public AWait OffsetRestart(float offset) => _timer.Set(_waitTime + offset);

		[Impl(256)] sealed public override void Reset() => _timer.Set(_waitTime);

		#region Nested Timer, AConverter
		// *******************************************************
		sealed private class Timer : AWait
		{
			private readonly Func<float> _applicationTime;
			private float _waitUntilTime;
			private bool _isWait;

			public override bool IsWait { [Impl(256)] get => _isWait; }

			public Timer(Func<float> applicationTime) => _applicationTime = applicationTime;

			[Impl(256)] public override bool MoveNext() => _isWait = _waitUntilTime > _applicationTime();

			[Impl(256)] public AWait Set(float time)
			{
				_waitUntilTime = time + _applicationTime();
				return this;
			}
		}
		// *******************************************************
		public abstract class AConverter : AJsonConverter<AWaitTime>
		{
			sealed public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				return TimerCreate(serializer.Deserialize<float>(reader));
			}

			protected abstract object TimerCreate(float time);

			sealed protected override void WriteJson(JsonWriter writer, AWaitTime value, JsonSerializer serializer)
			{
				writer.WriteValue(value._waitTime);
			}
		}
		// *******************************************************
		#endregion
	}
}
