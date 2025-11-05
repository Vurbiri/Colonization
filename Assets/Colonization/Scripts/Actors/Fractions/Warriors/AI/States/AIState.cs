using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        private abstract class AIState : State<WarriorAI>
        {
            protected Id<PlayerId> _playerId;

            protected ReadOnlyReactiveList<Crossroad> Colonies { [Impl(256)] get => GameContainer.Players.Humans[_playerId].Colonies; }

            [Impl(256)]
            protected AIState(WarriorAI parent) : base(parent)
            {
                _playerId = parent._actor.Owner;
            }
        }
    }
}
