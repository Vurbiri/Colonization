//Assets\Colonization\Scripts\Actors\Fractions\Warriors\WarriorsSpawner.cs
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;

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

        public Warrior Create(Id<WarriorId> id, IReactive<IPerk> artefact, Hexagon startHex)
        {
            IReactive<IPerk>[] buffs = { artefact };
            return Object.Instantiate(_warriorPrefab, _container).Init(id, _payerId, buffs, _material, startHex);
        }

        public Warrior Load(ActorLoadData data, IReactive<IPerk> artefact, Hexagons land)
        {
            IReactive<IPerk>[] buffs = { artefact };
            return Object.Instantiate(_warriorPrefab, _container).Load(data, _payerId, buffs, _material, land[data.keyHex]);
        }
    }
}
