using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon
    {
        public partial class FattyStates
        {
            sealed private class JumpState : AActionState
            {
                private readonly HitEffects _effects;
                private readonly List<Actor> _targets = new();
                private readonly Chance _chance;
                private bool _canUse;

                public new bool CanUse
                {
                    get
                    {
                        _targets.Clear();
                        if(base.CanUse && _chance.Roll)
                        {
                            foreach (var hex in CurrentHex.Neighbors)
                            {
                                if (hex.IsWarrior)
                                    _targets.Add(hex.Owner);
                            }
                        }

                        return _canUse = _targets.Count > 1; 
                    }
                }

                public JumpState(SpecSkillSettings specSkill, FattyStates parent) : base(parent, CONST.SPEC_SKILL_ID, specSkill.Cost)
                {
                    _effects = specSkill.HitEffects[0];
                    _chance = new(specSkill.Value);
                }

                public override void Enter()
                {
                    if(_canUse)
                        StartCoroutine(ApplySkill_Cn());
                    else
                        GetOutOfThisState();
                }

                private IEnumerator ApplySkill_Cn()
                {
                    _canUse = false;
                    var wait = Skin.SpecAttack();

                    yield return wait; wait.Reset();

                    Actor target;
                    for (int i = _targets.Count -  1; i >= 0; i--)
                    {
                        target = _targets[i];
                        _effects.Apply(Actor, target);
                        target.Skin.Impact(null);
                    }

                    yield return wait;
                    GetOutOfThisState();
                }
            }
        }
    }

}
