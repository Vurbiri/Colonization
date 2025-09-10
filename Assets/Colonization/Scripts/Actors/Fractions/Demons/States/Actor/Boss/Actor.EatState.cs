using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class Demon
    {
        public partial class BossStates
        {
            sealed private class EatState : AActionState
            {
                private readonly List<Crossroad> _targets = new(HEX.SIDES);
                private readonly ReadOnlyArray<HitEffects> _effects;
                private readonly int _hpOffset;
                private bool _canUse;

                public new bool CanUse
                {
                    get
                    {
                        _targets.Clear();
                        //if (base.CanUse && !Chance.Rolling(HP.Percent + _hpOffset))
                        //{
                        //    var crossroads = CurrentHex.Crossroads;
                        //    Crossroad crossroad;
                        //    for (int i = 0; i < HEX.SIDES; i++)
                        //    {
                        //        crossroad = crossroads[i];
                        //        if (crossroad.IsColony && GameContainer.Players[crossroad.Owner].Resources.Amount > 0)
                        //            _targets.Add(crossroad);
                        //    }
                        //}

                        _targets.AddRange(CurrentHex.Crossroads);

                        return _canUse = _targets.Count > 0;
                    }
                }

                public EatState(SpecSkillSettings specSkill, BossStates parent) : base(parent, CONST.SPEC_SKILL_ID, specSkill.Cost)
                {
                    _effects = specSkill.HitEffects;
                    _hpOffset = specSkill.Value;
                }
                public override void Enter()
                {
                    if (_canUse)
                        StartCoroutine(ApplySkill_Cn());
                    else
                        GetOutOfThisState();
                }

                private IEnumerator ApplySkill_Cn()
                {
                    _canUse = false;

                    //var colony = _targets.Rand();
                    var colony = _targets[0];
                    Transform.LookAt(colony.Position, Vector3.up);

                    var wait = Skin.SpecSkill();

                    yield return wait; wait.Reset();

                    for (int i = 0; i < 2; i++)
                        _effects[i].Apply(Actor, Actor);

                    yield return wait;
                    GetOutOfThisState();
                }

            }
        }
    }
}
