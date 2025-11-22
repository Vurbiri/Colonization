using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private class Commander : Commander<WarriorAI>
        {
            public Commander(ReadOnlyReactiveSet<Actor> actors) : base(CONST.DEFAULT_MAX_WARRIOR)
            {
                WarriorAI.Start();
                actors.Subscribe(OnActor);
            }

            protected override WarriorAI GetActorAI(Actor actor) => new(actor, _goals);
        }
    }
}
