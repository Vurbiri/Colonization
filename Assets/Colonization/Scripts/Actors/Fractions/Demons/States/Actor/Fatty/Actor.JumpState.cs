using System.Collections;

namespace Vurbiri.Colonization
{
    sealed public partial class Demon
    {
        public partial class FattyStates
        {
            sealed private class JumpState : AActionState
            {
                private readonly HitEffects _effects;
                private Chance _chance;
 
                public override bool CanUse => base.CanUse && _chance.Roll;

                public JumpState(SpecSkillSettings specSkill, FattyStates parent) : base(parent, CONST.SPEC_SKILL_ID, specSkill.Cost)
                {
                    _effects = specSkill.HitEffects[0];
                    _chance = specSkill.Value;
                }

                public override void Enter()
                {
                    StartCoroutine(ApplySkill_Cn());
                }

                private IEnumerator ApplySkill_Cn()
                {
                    yield return GameContainer.CameraController.ToPositionControlled(CurrentHex.Position);

                    var wait = Skin.SpecAttack();

                    yield return wait; 
                    wait.Reset();

                    Actor target;
                    var neighbors = CurrentHex.Neighbors;
                    for (int i = 0; i < HEX.SIDES; ++i)
                    {
                        if (neighbors[i].IsWarrior)
                        {
                            target = neighbors[i].Owner;
                            _effects.Apply(Actor, target);
                            target.Skin.Impact(null);
                        }
                    }
                    Pay();

                    yield return wait;

                    ToExit();
                }
            }
        }
    }

}
