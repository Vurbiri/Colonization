using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
	public partial class SpellBook
	{
        sealed private class HealRandomActor : ASpell
        {
            private readonly Effect _heal;
            private readonly IHitSFX _sfx;
            private readonly HashSet<Actor> _wounded = new(CONST.DEFAULT_MAX_ACTORS * PlayerId.Count);

            public HealRandomActor(IHitSFX sfx)
            {
				_heal = new(ActorAbilityId.CurrentHP, TypeModifierId.TotalPercent, s_settings.healRandomValue);
                _sfx = sfx;

                for (int i = 0; i < PlayerId.Count; i++)
                    s_actors[i].Subscribe(OnActor);
            }

            public override bool Cast(SpellParam param)
            {
                if (canUse)
                    s_coroutines.Run(Cast_Cn(_wounded.RandE()));
                return canUse;
            }

            private IEnumerator Cast_Cn(Actor target)
            {
                yield return s_cameraController.ToPosition(target.Position);
                yield return _sfx.Hit(target.Skin);
                target.ApplyEffect(_heal);
            }

            private void OnActor(Actor actor, TypeEvent operation)
            {
                if (operation == TypeEvent.Subscribe | operation == TypeEvent.Change)
                {
                    if (actor.IsWounded)
                        _wounded.Add(actor);
                    else
                        _wounded.Remove(actor);
                }
                else if(operation == TypeEvent.Remove)
                {
                    _wounded.Remove(actor);
                }
                canUse = _wounded.Count > 0;
            }
        }
	}
}
