using System.Collections;
using static Vurbiri.Colonization.Actor;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class BlockInCombat : State
        {
            private readonly int _blockCost;

            [Impl(256)] public BlockInCombat(AI<WarriorsAISettings, WarriorId, WarriorAIStateId> parent) : base(parent) => _blockCost = Action.GetCostSkill(CONST.SPEC_SKILL_ID);

            public override bool TryEnter()
            {
                return IsInCombat && Actor.CurrentAP == _blockCost && Actor.PercentHP < s_settings.maxHPForBlock && Action.CanUsedSpecSkill() &&
                    Chance.Rolling((Status.nearEnemies.Force * s_settings.ratioForBlock) / (Actor.CurrentForce * (Hexagon.GetMaxDefense() + _blockCost)));
            }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return Action.UseSpecSkill();

                isContinue.Set(false);
                Exit();
            }

            public override void Dispose() { }
        }
    }
}
