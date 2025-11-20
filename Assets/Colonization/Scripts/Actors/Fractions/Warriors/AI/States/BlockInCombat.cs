using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class BlockInCombat : State<WarriorAI>
        {
            private readonly int _blockCost;
            
            public override int Id => WarriorAIStateId.BlockInCombat;

            [Impl(256)] public BlockInCombat(WarriorAI parent) : base(parent) => _blockCost = Action.GetCostSkill(CONST.SPEC_SKILL_ID);

            public override bool TryEnter()
            {
                bool isBlock = false;
                if (IsInCombat && Actor.CurrentAP == _blockCost)
                {
                    int selfForce = Actor.CurrentForce;
                    if (Status.isGuard)
                        selfForce *= (Hexagon.GetMaxDefense() + 1);

                    isBlock = Chance.Rolling((Status.nearEnemies.Force * s_settings.ratioForBlock) / selfForce);
                }
                
                return isBlock;
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
