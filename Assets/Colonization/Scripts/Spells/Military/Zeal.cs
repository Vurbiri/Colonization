using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
	public partial class SpellBook
	{
        sealed private class Zeal : ASpell
        {
            private readonly Effect _heal;
            private readonly IHitSFX _sfx;
            private readonly List<Actor> _wounded = new(CONST.DEFAULT_MAX_WARRIOR * PlayerId.Count);

            private Zeal(IHitSFX sfx, int type, int id) : base(type, id)
            {
				_heal = new(ActorAbilityId.CurrentHP, TypeModifierId.TotalPercent, s_settings.zealPercentHeal);
                _sfx = sfx;
            }
            public static void Create(IHitSFX sfx) => new Zeal(sfx, MilitarySpellId.Type, MilitarySpellId.Zeal);

            public override void Cast(SpellParam param)
            {
                _wounded.Clear();
                for (int i = 0; i < PlayerId.Count; i++)
                {
                    foreach (Actor actor in s_actors[i])
                    {
                        if (actor.IsWounded)
                            _wounded.Add(actor);
                    }
                }

                if (_wounded.Count > 0)
                    Cast_Cn(_wounded.Rand(), param.playerId).Start();
            }


            private IEnumerator Cast_Cn(Actor target, int playerId)
            {
                s_isCast.True();

                yield return GameContainer.CameraController.ToPosition(target.Position);
                _sfx.Hit(null, target.Skin);
                target.ApplyEffect(_heal);

                s_isCast.False();
            }
        }
	}
}
