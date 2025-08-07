using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class RandomHealing : ASpell
        {
            private readonly List<Actor> _wounded = new(CONST.DEFAULT_MAX_WARRIOR << 2);
            private readonly string _strCost;
            private readonly Effect _heal;

            private RandomHealing(int type, int id) : base(type, id) 
            {
                _heal = new(ActorAbilityId.CurrentHP, TypeModifierId.TotalPercent, s_settings.healPercentValue);

                _strCost = "\n".Concat(string.Format(TAG.CURRENCY, CurrencyId.Mana, _cost[CurrencyId.Mana]));
            }
            public static void Create() => new RandomHealing(EconomicSpellId.Type, EconomicSpellId.RandomHealing);

            public override bool Prep(SpellParam param)
            {
                _wounded.Clear();
                if(!s_isCast && s_humans[param.playerId].IsPay(_cost))
                {
                    for(int playerId = 0; playerId < PlayerId.Count; playerId++)
                    {
                        foreach (Actor actor in s_actors[playerId])
                        {
                            if(actor.IsWounded)
                                _wounded.Add(actor);
                        }
                    }
                }
                return _canCast = _wounded.Count > 0;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    s_isCast.True();
                    
                    Cast_Cn(_wounded.Rand()).Start();
                    ShowNameSpell(param.playerId);
                    s_humans[param.playerId].Pay(_cost);

                    _canCast = false;
                }
            }

            private IEnumerator Cast_Cn(Actor target)
            {
                yield return GameContainer.CameraController.ToPosition(target.Position, true); ;

                target.ApplyEffect(_heal);
                GameContainer.HitSFX.Hit(s_settings.healSFX, s_sfxUser, target.Skin);

                s_isCast.False();
            }

            protected override string GetDesc(Localization localization)
            {
                return string.Concat(localization.GetFormatText(FILE, _descKey, s_settings.orderPerMana), _strCost);
            }
        }
    }
}
