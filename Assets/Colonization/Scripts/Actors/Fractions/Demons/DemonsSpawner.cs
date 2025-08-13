using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class DemonsSpawner
	{
        private readonly ActorInitData _initData;
        private readonly Hexagon _startHex;

        private int _potential;

        public int Potential => _potential;

        public DemonsSpawner(IReactiveValue<int> level, ActorInitData initData, Hexagon startHex, int potential)
        {
            _initData = initData;
            _startHex = startHex;
            _potential = potential;

            level.Subscribe(value => _potential += value, false);
        }

        public bool TryCreate(out Actor demon)
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

            demon = GameContainer.ActorsFactory.Create(ActorTypeId.Demon, id, _initData, _startHex);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Actor Load(ActorLoadData loadData) => GameContainer.ActorsFactory.Load(ActorTypeId.Demon, _initData, loadData);
    }
}
