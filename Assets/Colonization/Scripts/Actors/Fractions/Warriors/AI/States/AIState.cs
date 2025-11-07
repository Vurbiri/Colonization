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
            protected bool IsBlock { [Impl(256)] get => Action.CanUseSpecSkill() && _parent._blockChance.Roll; }
            protected bool IsDefenseBuff
            {
                [Impl(256)]
                get 
                {
                    var used = s_settings.defenseBuff[Actor.Id];
                    return Action.CanUseSkill(used.skill) && used.chance.Roll; 
                }
            }
            

            [Impl(256)]
            protected AIState(WarriorAI parent) : base(parent)
            {
                _playerId = parent._actor.Owner;
            }

            protected IEnumerator Defense_Cn(bool isBuff, bool isBlock)
            {
                var used = s_settings.defenseBuff[Actor.Id];
                if (isBuff && Action.CanUseSkill(used.skill) && used.chance.Roll)
                    yield return Action.UseSkill(used.skill);
                if (isBlock && Action.CanUseSpecSkill() && _parent._blockChance.Roll)
                    yield return Action.UseSpecSkill();
            }
        }
    }
}
