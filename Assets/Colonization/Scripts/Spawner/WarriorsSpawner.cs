//Assets\Colonization\Scripts\Spawner\WarriorsSpawner.cs
using UnityEngine;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization.Actors
{
    public class WarriorsSpawner
    {
        private readonly int _payerId;
        private readonly WarriorInitializer _warriorPrefab;
        private readonly Material _material;
        private readonly Transform _container;

        public WarriorsSpawner(int payerId, WarriorInitializer warriorPrefab, Material material, Transform container)
        {
            _payerId = payerId;
            _warriorPrefab = warriorPrefab;
            _material = material;
            _container = container;
        }

        public Warrior Create(int id, Hexagon startHex)
        {
            WarriorInitializer warrior = Object.Instantiate(_warriorPrefab, _container);
            return warrior.Init(id, _payerId, _material, startHex);
        }

        public Warrior Create(ActorLoadData data, Land land)
        {
            WarriorInitializer warrior = Object.Instantiate(_warriorPrefab, _container);
            return warrior.Init(data, _payerId, _material, land[data.keyHex]);
        }
    }
}
