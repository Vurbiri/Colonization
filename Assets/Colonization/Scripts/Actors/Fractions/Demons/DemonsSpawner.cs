//Assets\Colonization\Scripts\Actors\Fractions\Demons\DemonsSpawner.cs
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class DemonsSpawner
	{
        private readonly DemonInitializer _demonPrefab;
        private readonly Hexagon _startHex;
        private readonly Transform _container;

        public DemonsSpawner(DemonInitializer demonPrefab, Transform container, Hexagon startHex)
        {
            _demonPrefab = demonPrefab;
            _container = container;
            _startHex = startHex;
        }

        public Demon Create(int id, IReactive<IPerk> artefact, IReactive<IPerk> demonBuffs)
        {
            IReactive<IPerk>[] buffs = { artefact, demonBuffs };
            return Object.Instantiate(_demonPrefab, _container).Init(id, buffs, _startHex);
        }

        public Demon Load(ActorLoadData data, IReactive<IPerk> artefact, IReactive<IPerk> demonBuffs, Hexagons land)
        {
            IReactive<IPerk>[] buffs = { artefact, demonBuffs };
            return Object.Instantiate(_demonPrefab, _container).Load(data, buffs, land[data.keyHex]);
        }
    }
}
