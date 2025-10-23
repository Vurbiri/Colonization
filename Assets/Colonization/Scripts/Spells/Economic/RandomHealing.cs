using System.Collections;
using System.Collections.Generic;
using Vurbiri.International;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class RandomHealing : ASpell
        {
            private readonly List<Actor> _wounded = new(CONST.DEFAULT_MAX_WARRIOR << 2);
            private readonly Effect _heal;

            private RandomHealing(int type, int id) : base(type, id) 
            {
                _heal = new(ActorAbilityId.CurrentHP, TypeModifierId.TotalPercent, s_settings.healPercentValue);

                SetManaCost();
            }
            public static void Create() => new RandomHealing(EconomicSpellId.Type, EconomicSpellId.RandomHealing);

            public override bool Prep(SpellParam param)
            {
                if (_canCast = !s_isCasting)
                {
                    _wounded.Clear();
                    if (Humans[param.playerId].IsPay(_cost))
                    {
                        for (int playerId = 0; playerId < PlayerId.Count; playerId++)
                        {
                            foreach (var actor in GameContainer.Actors[playerId])
                            {
                                if (actor.IsWounded) _wounded.Add(actor);
                            }
                        }
                    }
                    _canCast = _wounded.Count > 0;
                }
                return _canCast;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    s_isCasting.True();
                    
                    Cast_Cn(_wounded.Rand()).Start();
                    ShowSpellName(param.playerId);
                    Humans[param.playerId].Pay(_cost);

                    _canCast = false;
                }
            }

            private IEnumerator Cast_Cn(Actor target)
            {
                yield return GameContainer.CameraController.ToPositionControlled(target);

                target.ApplyEffect(_heal);
                GameContainer.SFX.Run(s_settings.healSFX, null, target.Skin);

                s_isCasting.False();
            }

            protected override string GetDesc(Localization localization)
            {
                return string.Concat(localization.GetFormatText(FILE, _descKey, s_settings.healPercentValue), _strCost);
            }
        }
    }
}
