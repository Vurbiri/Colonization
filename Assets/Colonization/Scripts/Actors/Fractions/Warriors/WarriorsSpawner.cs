//Assets\Colonization\Scripts\Actors\Fractions\Warriors\WarriorsSpawner.cs
using UnityEngine;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization.Actors
{
    public class WarriorsSpawner
    {
        private readonly Id<PlayerId> _payerId;
        private readonly WarriorInitializer _warriorPrefab;
        private readonly Material _material;
        private readonly Transform _container;

        public WarriorsSpawner(Id<PlayerId> payerId, WarriorInitializer warriorPrefab, Material material, Transform container)
        {
            _payerId = payerId;
            _warriorPrefab = warriorPrefab;
            _material = material;
            _container = container;
        }

        public Warrior Create(int id, Hexagon startHex)
        {
            return Object.Instantiate(_warriorPrefab, _container).Init(id, _payerId, _material, startHex);
        }

        public Warrior Load(ActorLoadData data, Land land)
        {
            return Object.Instantiate(_warriorPrefab, _container).Load(data, _payerId, _material, land[data.keyHex]);
        }
    }
}
