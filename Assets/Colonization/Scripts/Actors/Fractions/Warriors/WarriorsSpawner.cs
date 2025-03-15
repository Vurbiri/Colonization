//Assets\Colonization\Scripts\Actors\Fractions\Warriors\WarriorsSpawner.cs
using UnityEngine;
using Vurbiri.Colonization.Data;

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

        public Warrior Load(ActorLoadData data, Hexagons land)
        {
            return Object.Instantiate(_warriorPrefab, _container).Load(data, _initData, _material, land[data.keyHex]);
        }
    }
}
