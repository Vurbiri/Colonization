using System.Collections;
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

            public IEnumerator TryCreate_Cn()
            {
                if (_potential != 0 && _startHex.IsEmpty)
                {
                    yield return GameContainer.CameraController.ToPositionControlled(Vector3.zero);

                    int minId = MathI.Min(_potential >> 2, DemonId.Fatty);
                    int maxId = MathI.Min(_potential, DemonId.Count);
                    int id = Random.Range(minId, maxId);

                    _potential -= (id + 1);
                    GameContainer.Actors.Create(ActorTypeId.Demon, id, _initData, _startHex).Skin.EventStart.Add(_waitSpawn.Send);
                    yield return _waitSpawn.Restart();
                }
                yield break;
            }

            [Impl(256)] public Actor Load(ActorLoadData loadData) => GameContainer.Actors.Load(ActorTypeId.Demon, _initData, loadData);

            [Impl(256)] public void AddPotential(int add) => _potential += add;
        }
    }
}
