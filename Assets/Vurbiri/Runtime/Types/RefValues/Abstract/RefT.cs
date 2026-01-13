using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable]
	public abstract class Ref<T>
	{
		[UnityEngine.SerializeField] protected T _value;

		public T Value { [Impl(256)] get => _value; }

        [Impl(256)] public override string ToString() => _value.ToString();

        [Impl(256)] public static implicit operator T(Ref<T> self) => self._value;
	}
}
