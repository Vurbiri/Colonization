using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
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
            return GameContainer.Actors.Create(ActorTypeId.Warrior, id, _initData, startHex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Actor CreateDemon(Id<DemonId> id, Hexagon startHex)
        {
            return GameContainer.Actors.Create(ActorTypeId.Demon, id, _initData, startHex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Actor Load(ActorLoadData loadData) => GameContainer.Actors.Load(ActorTypeId.Warrior, _initData, loadData);
    }
}
