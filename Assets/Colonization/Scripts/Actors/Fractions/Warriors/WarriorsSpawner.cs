using UnityEngine;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization.Actors
{
    public class WarriorsSpawner
    {
        private readonly ActorInitData _initData;
        private readonly WarriorInitializer _warriorPrefab;
        private readonly Transform _container;
        private readonly Material _material;

        public WarriorsSpawner(ActorInitData initData, WarriorInitializer warriorPrefab, Transform container)
        {
            _initData = initData;
            _warriorPrefab = warriorPrefab;
            _container = container;
            _material = GameContainer.Materials[initData.owner].materialWarriors;
        }

        public Warrior Create(Id<WarriorId> id, Hexagon startHex)
        {
            return Object.Instantiate(_warriorPrefab, _container).Init(id, _initData, _material, startHex);
        }

        public Warrior Load(ActorLoadData data)
        {
            return Object.Instantiate(_warriorPrefab, _container).Load(data, _initData, _material, GameContainer.Hexagons[data.keyHex]);
        }
    }
}
