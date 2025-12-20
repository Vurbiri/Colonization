using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class RefFloat
	{
		public float value;

        [Impl(256)] public RefFloat() { }
        [Impl(256)] public RefFloat(float value) => this.value = value;

        [Impl(256)] public static implicit operator RefFloat(float value) => new(value);
        [Impl(256)] public static implicit operator float(RefFloat value) => value.value;
    }
}
