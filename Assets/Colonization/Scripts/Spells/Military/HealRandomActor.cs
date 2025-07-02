using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
	public partial class SpellBook
	{
        private class HealRandomActor : ASpell
        {
            private readonly Effect _heal;
            private readonly IHitSFX _sfx;
            private readonly List<Actor> _wounded = new(CONST.DEFAULT_MAX_ACTORS * PlayerId.Count);

            public HealRandomActor(SpellBook book, IHitSFX sfx) : base(book)
            {
				_heal = new(ActorAbilityId.CurrentHP, TypeModifierId.TotalPercent, book._settings.healRandomValue);
                _sfx = sfx;
            }

            public override bool Init(int playerID)
            {
                _wounded.Clear();

                for (int i = 0; i < PlayerId.Count; i++)
                {
                    foreach (var actor in _book._actors[i])
                    {
                        if (actor.IsWounded)
                            _wounded.Add(actor);
                    }
                }

                return _wounded.Count > 0;
            }

            public override void Cast(SpellParameters param)
            {
                if (_wounded.Count > 0)
                    _book._coroutines.Run(Cast_Cn(_wounded.Rand()));
            }

            private IEnumerator Cast_Cn(Actor target)
            {
                yield return _book._cameraController.ToPosition(target.Position);
                yield return _sfx.Hit(target.Skin);
                target.ApplyEffect(_heal);
            }
        }
	}
}
