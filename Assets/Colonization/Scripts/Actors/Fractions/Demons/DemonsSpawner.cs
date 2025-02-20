//Assets\Colonization\Scripts\Actors\Fractions\Demons\DemonsSpawner.cs
using UnityEngine;
using Vurbiri.Colonization.Data;

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

        public Demon Create(int id, Hexagon startHex)
        {
            return Object.Instantiate(_demonPrefab, _container).Init(id, startHex);
        }

        public Demon Load(ActorLoadData data, Land land)
        {
            return Object.Instantiate(_demonPrefab, _container).Load(data, land[data.keyHex]);
        }
    }
}
