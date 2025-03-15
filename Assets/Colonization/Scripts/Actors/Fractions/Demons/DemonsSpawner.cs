//Assets\Colonization\Scripts\Actors\Fractions\Demons\DemonsSpawner.cs
using UnityEngine;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization.Actors
{
    public class DemonsSpawner
	{
        private readonly ActorInitData _initData;
        private readonly DemonInitializer _demonPrefab;
        private readonly Hexagon _startHex;
        private readonly Transform _container;

        public DemonsSpawner(ActorInitData initData, DemonInitializer demonPrefab, Transform container, Hexagon startHex)
        {
            _initData = initData;
            _demonPrefab = demonPrefab;
            _container = container;
            _startHex = startHex;
        }

        public Demon Create(int id)
        {
            return Object.Instantiate(_demonPrefab, _container).Init(id, _initData, _startHex);
        }

        public Demon Load(ActorLoadData data, Hexagons land)
        {
            return Object.Instantiate(_demonPrefab, _container).Load(data, _initData, land[data.keyHex]);
        }
    }
}
