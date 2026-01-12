using Newtonsoft.Json;
using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive
{
	[Serializable, JsonConverter(typeof(Converter))]
	sealed public class ReactiveBool : ReactiveValue<bool>
	{
		[Impl(256)] public ReactiveBool() : base(false) { }
		[Impl(256)] public ReactiveBool(bool value) : base(value) { }

		[Impl(256)] public void True()
		{
			if (!_value) _onChange.Invoke(_value = true);
		}
		[Impl(256)] public void False()
		{
			if (_value)  _onChange.Invoke(_value = false);
		}
		[Impl(256)] public void Negation() => _onChange.Invoke(_value = !_value);

		[Impl(256)] public static implicit operator bool(ReactiveBool value) => value._value;

		#region Logic operator
		public static bool operator !(ReactiveBool r) => !r._value;

		public static bool operator |(ReactiveBool a, ReactiveBool b) => a._value | b._value;
		public static bool operator |(ReactiveBool r, IReactiveValue<bool> i) => r._value | i.Value;
		public static bool operator |(IReactiveValue<bool> i, ReactiveBool r) => i.Value | r._value;
		public static bool operator |(ReactiveBool r, bool i) => r._value | i;
		public static bool operator |(bool i, ReactiveBool r) => i | r._value;

		public static bool operator &(ReactiveBool a, ReactiveBool b) => a._value & b._value;
		public static bool operator &(ReactiveBool r, IReactiveValue<bool> i) => r._value & i.Value;
		public static bool operator &(IReactiveValue<bool> i, ReactiveBool r) => i.Value & r._value;
		public static bool operator &(ReactiveBool r, bool i) => r._value & i;
		public static bool operator &(bool i, ReactiveBool r) => i & r._value;

		public static bool operator ^(ReactiveBool a, ReactiveBool b) => a._value ^ b._value;
		public static bool operator ^(ReactiveBool r, IReactiveValue<bool> i) => r._value ^ i.Value;
		public static bool operator ^(IReactiveValue<bool> i, ReactiveBool r) => i.Value ^ r._value;
		public static bool operator ^(ReactiveBool r, bool i) => r._value ^ i;
		public static bool operator ^(bool i, ReactiveBool r) => i ^ r._value;
		#endregion

		#region Nested JsonConverter
		//***************************************************
		sealed public class Converter : AJsonConverter<ReactiveBool>
		{
			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				return new ReactiveBool(serializer.Deserialize<bool>(reader));
			}

			protected override void WriteJson(JsonWriter writer, ReactiveBool value, JsonSerializer serializer)
			{
				writer.WriteValue(value._value);
			}
		}
		#endregion
	}
}
