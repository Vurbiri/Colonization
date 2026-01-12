using Vurbiri.Colonization.Storage;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public abstract class ASpawner
	{
		protected readonly int _type;
		protected readonly ActorInitData _initData;

		[Impl(256)] protected ASpawner(int type, ActorInitData initData)
		{
			_type = type;
			_initData = initData;
		}

		[Impl(256)] public Actor Create(int id, Hexagon start) => GameContainer.Actors.Create(_type, id, _initData, start);

		[Impl(256)] public WaitSignal Load(ActorLoadData loadData) => GameContainer.Actors.Load(_type, _initData, loadData);
	}
}
