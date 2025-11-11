using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        private abstract class AIState : State<WarriorAI>
        {
            protected ReadOnlyReactiveList<Crossroad> Colonies { [Impl(256)] get => GameContainer.Players.Humans[OwnerId.Value].Colonies; }

            [Impl(256)] protected AIState(WarriorAI parent) : base(parent) { }


            

            [Impl(256)] protected ReadOnlyReactiveList<Crossroad> GetColonies(int playerId) => GameContainer.Players.Humans[playerId].Colonies;
        }
    }
}
