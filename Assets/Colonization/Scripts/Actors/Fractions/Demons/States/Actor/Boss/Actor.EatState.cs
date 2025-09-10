using System.Collections.Generic;
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
                        if (base.CanUse && !Chance.Rolling(HP.Percent + _hpOffset))
                        {
                            var crossroads = CurrentHex.Crossroads;
                            Crossroad crossroad;
                            for (int i = 0; i < HEX.SIDES; i++)
                            {
                                crossroad = crossroads[i];
                                //if (crossroad.IsColony && GameContainer.Players[crossroad.Owner].r)
                            }
                        }

                        return _canUse = _targets.Count > 0;
                    }
                }

                public EatState(SpecSkillSettings specSkill, BossStates parent) : base(parent, CONST.SPEC_SKILL_ID, specSkill.Cost)
                {
                    _effects = specSkill.HitEffects;
                    _hpOffset = specSkill.Value;
                }


            }
        }
    }
}
