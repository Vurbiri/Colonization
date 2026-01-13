using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Collections
{
	public class Version
	{
		protected uint _version;

		[Impl(256)] public void Next() => _version = unchecked(++_version);

		[Impl(256)] public static implicit operator Current(Version version) => new(version);

		//--------------- Nested --------------------
		public readonly struct Current
		{
			private readonly Version _parent;
			private readonly uint _version;

			[Impl(256)] public Current(Version parent)
			{
				_parent = parent;
				_version = parent._version;
			}

			[Impl(256)] public readonly void Verify()
			{
				if (_version != _parent._version)
					Errors.InvalidOperation("Collection was modified; enumeration operation may not execute.");
			}
		}
		//--------------------------------------------
	}
}
