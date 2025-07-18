using UnityEngine;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class DemonsSpawner
	{
        private const int SHIFT_ID = 5;
        
        private readonly ActorInitData _initData;
        private readonly DemonInitializer _demonPrefab;
        private readonly Hexagon _startHex;
        private readonly Transform _container;

        private int _potential;

        public int Potential => _potential;

        public DemonsSpawner(IReactiveValue<int> level, ActorInitData initData, Player.Settings settings, Hexagon startHex, int potential)
        {
            _initData = initData;
            _demonPrefab = settings.demonPrefab;
            _container = settings.actorsContainer;
            _startHex = startHex;
            _potential = potential;

            level.Subscribe(value => _potential += value, false);
        }

        public bool TryCreate(out Demon demon)
        {
            if (_potential == 0 | _startHex.IsOwner)
            {
                demon = null;
                return false;
            }

            int maxId = Mathf.Min(_potential, DemonId.Count);
            int id = Random.Range(0, maxId << SHIFT_ID) % maxId;
            _potential -= (id + 1);

            demon = Object.Instantiate(_demonPrefab, _container).Init(id, _initData, _startHex);
            return true;
        }

        public Demon Load(ActorLoadData data, Hexagons land)
        {
            return Object.Instantiate(_demonPrefab, _container).Load(data, _initData, land[data.keyHex]);
        }
    }
}
