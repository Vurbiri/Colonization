using UnityEngine;
using Vurbiri.Colonization.Storage;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class DemonsSpawner
	{
        private readonly ActorInitData _initData;
        private readonly Hexagon _startHex;

        private int _potential;

        public int Potential { [Impl(256)] get => _potential; }

        public DemonsSpawner(ActorInitData initData, Hexagon startHex, int potential)
        {
            _initData = initData;
            _startHex = startHex;
            _potential = potential;
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

            demon = GameContainer.Actors.Create(ActorTypeId.Demon, id, _initData, _startHex);
            return true;
        }

        [Impl(256)] public Actor Load(ActorLoadData loadData) => GameContainer.Actors.Load(ActorTypeId.Demon, _initData, loadData);

        [Impl(256)] public void AddPotential(int potential) => _potential += potential;
    }
}
