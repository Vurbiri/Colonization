using System.Collections;
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
            protected bool IsBlock { [Impl(256)] get => Action.CanUsedSpecSkill() && _parent._blockChance.Roll; }
            

            [Impl(256)]
            protected AIState(WarriorAI parent) : base(parent)
            {
                _playerId = parent._actor.Owner;
            }

            [Impl(256)] protected ReadOnlyReactiveList<Crossroad> GetColonies(int playerId) => GameContainer.Players.Humans[playerId].Colonies;

            protected IEnumerator Defense_Cn(bool isBuff, bool isBlock)
            {
                var skill = s_settings.defenseBuff[Actor.Id];
                if (isBuff && skill.CanUsed(Action, Actor))
                    yield return skill.Use(Action);
                if (isBlock && Action.CanUsedSpecSkill() && _parent._blockChance.Roll)
                    yield return Action.UseSpecSkill();
            }
        }
    }
}
