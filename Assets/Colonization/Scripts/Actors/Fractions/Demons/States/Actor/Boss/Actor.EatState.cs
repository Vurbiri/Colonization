using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    sealed public partial class Demon
    {
        public partial class BossStates
        {
            sealed private class EatState : AActionState
            {
                private readonly List<Crossroad> _targets = new(HEX.SIDES);
                private readonly ReadOnlyArray<HitEffects> _effects;
                private readonly Chance _chance;

                public override bool CanUse
                {
                    get
                    {
                        _targets.Clear();
                        if (base.CanUse && !ActorEffects.Contains(code) && _chance.Roll)
                        {
                            Crossroad crossroad;
                            var crossroads = CurrentHex.Crossroads;
                            for (int i = 0; i < HEX.SIDES; ++i)
                            {
                                crossroad = crossroads[i];
                                if (crossroad.IsColony && GameContainer.Humans[crossroad.Owner].Resources.Amount > 0)
                                    _targets.Add(crossroad);
                            }
                        }

                        return _targets.Count > 0;
                    }
                }

                public EatState(SpecSkillSettings specSkill, BossStates parent) : base(parent, CONST.SPEC_SKILL_ID, specSkill.Cost)
                {
                    _effects = specSkill.HitEffects;
                    _chance = new(specSkill.Value);
                }
                public override void Enter()
                {
                    if (_targets.Count > 0)
                        StartCoroutine(ApplySkill_Cn());
                    else
                        GetOutOfThisState();
                }

                public override void Exit()
                {
                    _targets.Clear();
                    signal.Send();
                }

                private IEnumerator ApplySkill_Cn()
                {
                    yield return GameContainer.CameraController.ToPositionControlled(CurrentHex.Position);

                    int countBuff = 1;
                    var colony = _targets.Rand();
                    Transform.LookAt(colony.Position, Vector3.up);

                    var wait = Skin.SpecSkill();

                    yield return wait; wait.Reset();
                    var resources = GameContainer.Humans[colony.Owner].Resources;
                    resources.RandomDecrement();
                    if (!colony.IsWall && resources.Amount > 0)
                    {
                        resources.RandomDecrement();
                        countBuff = 2;
                    }

                    yield return wait; wait.Reset();
                    for (int i = 0; i < countBuff; ++i)
                        _effects[i].Apply(Actor, Actor);
                    Pay();

                    yield return wait;

                    ToExit();
                }
            }
        }
    }
}
