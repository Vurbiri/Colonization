using UnityEngine;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization.Actors
{
    public class WarriorsSpawner
    {
        private readonly ActorInitData _initData;
        private readonly WarriorInitializer _warriorPrefab;
        private readonly Material _material;
        private readonly Transform _container;

        public WarriorsSpawner(ActorInitData initData, WarriorInitializer warriorPrefab, Material material, Transform container)
        {
            _initData = initData;
            _warriorPrefab = warriorPrefab;
            _material = material;
            _container = container;
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
