using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class WarriorsSpawner : ASpawner
	{
		[Impl(256)] public WarriorsSpawner(ActorInitData initData) : base(ActorTypeId.Warrior, initData) { }
	}
}
