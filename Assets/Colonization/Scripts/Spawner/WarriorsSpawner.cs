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
    }
}
