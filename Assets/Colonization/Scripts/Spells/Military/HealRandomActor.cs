using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
	public partial class SpellBook
	{
        sealed private class HealRandomActor : ASharedSpell
        {
            private readonly Effect _heal;
            private readonly IHitSFX _sfx;
            private readonly List<Actor> _wounded = new(CONST.DEFAULT_MAX_ACTORS * PlayerId.Count);

            private HealRandomActor(IHitSFX sfx)
            {
				_heal = new(ActorAbilityId.CurrentHP, TypeModifierId.TotalPercent, s_settings.healRandomValue);
                _sfx = sfx;
            }

            public static void Create(IHitSFX sfx) => s_sharedSpells[TypeOfPerksId.Military][MilitarySpellId.HealRandom] = new HealRandomActor(sfx);

            public override bool Cast(SpellParam param, CurrenciesLite resources)
            {
                _wounded.Clear();
                for (int i = 0; i < PlayerId.HumansCount; i++)
                {
                    foreach (Actor actor in s_actors[i])
                    {
                        if (actor.IsWounded)
                            _wounded.Add(actor);
                    }
                }

                if (_wounded.Count > 0)
                {
                    s_coroutines.Run(Cast_Cn(_wounded.Rand()));
                    return true;
                }

                return false;
            }

            private IEnumerator Cast_Cn(Actor target)
            {
                yield return s_cameraController.ToPosition(target.Position);
                yield return _sfx.Hit(target.Skin);
                target.ApplyEffect(_heal);
            }
        }
	}
}
