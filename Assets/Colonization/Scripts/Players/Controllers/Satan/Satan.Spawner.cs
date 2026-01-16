using System.Collections;
using UnityEngine;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public partial class Satan
	{
		public class Spawner : ASpawner
		{
			private readonly WaitSignal _waitSpawn = new();
			private readonly Hexagon _startHex;

			private int _potential;

            public ReadOnlyReactiveSet<Actor> Demons { [Impl(256)] get => GameContainer.Actors[PlayerId.Satan]; }
            public int Potential { [Impl(256)] get => _potential; }
            public bool CanSpawn { [Impl(256)] get => _potential > 0 && Demons.Count < CONST.MAX_DEMONS && _startHex.IsEmpty; }

            public Spawner(ActorInitData initData, int potential) : base(ActorTypeId.Demon, initData)
			{
				_startHex = GameContainer.Hexagons[Key.Zero];
				_potential = potential;
			}

			public IEnumerator TryCreate_Cn()
			{
#if TEST_AI
                UnityEngine.Debug.Log($"[SatanSpawner] Potential {_potential}");
#endif

                if (CanSpawn)
				{
					yield return GameContainer.CameraController.ToPositionControlled(Vector3.zero);

					int minId = MathI.Min(_potential / s_parameters.potential.minRatio, DemonId.Fatty);
					int maxId = MathI.Min(_potential, DemonId.Count);
					int id = Random.Range(minId, maxId);

					_potential -= (id + 1);
					GameContainer.Actors.Create(ActorTypeId.Demon, id, _initData, _startHex).Skin.EventStart.Add(_waitSpawn.Send);
					yield return _waitSpawn.Restart();
				}
				yield break;
			}

			[Impl(256)] public void AddPotential(int add) => _potential += add;
		}
	}
}
