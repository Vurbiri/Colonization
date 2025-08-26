using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization.Actors
{
    public class WarriorsSpawner
    {
        private readonly ActorInitData _initData;

        public WarriorsSpawner(ActorInitData initData)
        {
            _initData = initData;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Actor Create(Id<WarriorId> id, Hexagon startHex)
        {
            return GameContainer.ActorsFactory.Create(ActorTypeId.Warrior, id, _initData, startHex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Actor CreateDemon(Id<DemonId> id, Hexagon startHex)
        {
            return GameContainer.ActorsFactory.Create(ActorTypeId.Demon, id, _initData, startHex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Actor Load(ActorLoadData loadData) => GameContainer.ActorsFactory.Load(ActorTypeId.Warrior, _initData, loadData);
    }
}
