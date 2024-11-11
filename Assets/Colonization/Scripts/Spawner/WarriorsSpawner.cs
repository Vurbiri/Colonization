namespace Vurbiri.Colonization.Actors
{
    using UnityEngine;

    public class WarriorsSpawner
    {
        private readonly int _payerId;
        private readonly WarriorInitializer _warriorPrefab;
        private readonly Material _material;
        private readonly GameplayEventBus _eventBus;
        private readonly Transform _container;

        public WarriorsSpawner(int payerId, WarriorInitializer warriorPrefab, Material material, Transform container)
        {
            _payerId = payerId;
            _warriorPrefab = warriorPrefab;
            _material = material;
            _eventBus = SceneServices.Get<GameplayEventBus>();
            _container = container;
        }

        public Warrior Create(int id, Hexagon startHex)
        {
            WarriorInitializer warrior = Object.Instantiate(_warriorPrefab, _container);
            return warrior.Init(id, _payerId, _material, startHex, _eventBus);
        }

        public Warrior Create(int[][] data, Land land)
        {
            WarriorInitializer warrior = Object.Instantiate(_warriorPrefab, _container);
            return warrior.Init(data[1][0], data, _payerId, _material, land[new Key(data[0])], _eventBus);
        }
    }
}
