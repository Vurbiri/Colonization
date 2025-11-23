using UnityEngine;
using Vurbiri.Colonization.Storage;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Satan
    {
        protected class Spawner
        {
            private readonly WaitSignal _waitSpawn = new();
            private readonly Hexagon _startHex;
            private readonly ActorInitData _initData;

            private int _potential;

            public int Potential { [Impl(256)] get => _potential; }
            public bool CanSpawn { [Impl(256)] get => _potential > 0 && _startHex.IsEmpty; }

            public Spawner(ActorInitData initData, int potential)
            {
                _startHex = GameContainer.Hexagons[Key.Zero];
                _initData = initData;
                _potential = potential;
            }

            public WaitSignal TryCreate_Wait()
            {
                if (_potential != 0 && _startHex.CanDemonEnter)
                {
                    int minId = Mathf.Min(_potential >> 2, DemonId.Fatty);
                    int maxId = Mathf.Min(_potential, DemonId.Count);
                    int id = Random.Range(minId, maxId);

                    _potential -= (id + 1);
                    var demon = GameContainer.Actors.Create(ActorTypeId.Demon, id, _initData, _startHex);
                    demon.AddWallDefenceEffect(s_parameters.gateDefense);
                    demon.Skin.EventStart.Add(_waitSpawn.Send);

                    GameContainer.CameraController.ToPositionControlled(Vector3.zero);
                    return _waitSpawn;
                }
                return null;
            }

            [Impl(256)] public Actor Load(ActorLoadData loadData) => GameContainer.Actors.Load(ActorTypeId.Demon, _initData, loadData);

            [Impl(256)] public void AddPotential(int add) => _potential += add;
        }
    }
}
