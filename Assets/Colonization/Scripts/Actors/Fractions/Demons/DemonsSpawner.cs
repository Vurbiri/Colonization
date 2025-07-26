using UnityEngine;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class DemonsSpawner
	{
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

            int minId = Mathf.Min(_potential >> 2, DemonId.Fatty);
            int maxId = Mathf.Min(_potential, DemonId.Count);
            int id = Random.Range(minId, maxId);
            _potential -= (id + 1);

            demon = Object.Instantiate(_demonPrefab, _container).Init(id, _initData, _startHex);
            return true;
        }

        public Demon Load(ActorLoadData data)
        {
            return Object.Instantiate(_demonPrefab, _container).Load(data, _initData, GameContainer.Hexagons[data.keyHex]);
        }
    }
}
