using System.Collections;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
	public partial class SatanController
	{
		sealed private class Commander : Commander<DemonAI>
		{
			private readonly Spawner _spawner;

			public Commander(ReadOnlyReactiveSet<Actor> actors, Spawner spawner) : base(CONST.DEFAULT_MAX_DEMONS)
			{
				DemonAI.Start();
				actors.Subscribe(OnActor);
				_spawner = spawner;
			}

			public override IEnumerator Execution_Cn()
			{
				do
				{
					yield return _spawner.TryCreate_Cn();
					yield return base.Execution_Cn();
				}
				while (_spawner.CanSpawn);
			}

			protected override DemonAI GetActorAI(Actor actor) => new(actor, _goals);
		}
	}
}
