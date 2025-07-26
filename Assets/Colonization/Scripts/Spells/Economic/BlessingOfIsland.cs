using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.Characteristics.ReactiveEffectsFactory;
using static Vurbiri.Colonization.CurrencyId;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class BlessingOfIsland : ASpell
        {
            private readonly EffectCode _attackEffectCode = new(SPELL_TYPE, EconomicSpellId.Type, BLESS_SKILL_ID, 0);
            private readonly EffectCode _defenseEffectCode = new(SPELL_TYPE, EconomicSpellId.Type, BLESS_SKILL_ID, 1);
            private readonly List<Actor> _blessed = new(8);
            private readonly IHitSFX _sfx;

            private BlessingOfIsland(IHitSFX sfx, int type, int id) : base(type, id)
            {
                _sfx = sfx;
            }
            public static void Create(IHitSFX sfx) => new BlessingOfIsland(sfx, EconomicSpellId.Type, EconomicSpellId.Blessing);

            public override bool Prep(SpellParam param)
            {
                _blessed.Clear();
                _cost.Set(Gold, param.valueA); _cost.Set(Food, param.valueB);

                if (s_humans[param.playerId].IsPay(_cost))
                {
                    for (int i = 0, surface; i < PlayerId.Count; i++)
                    {
                        foreach (Actor actor in s_actors[i])
                        {
                            surface = actor.Hexagon.SurfaceId;
                            if (surface == SurfaceId.Village | surface == SurfaceId.Field)
                                _blessed.Add(actor);
                        }
                    }
                }
                return _canCast = _blessed.Count > 0;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    int value = Mathf.RoundToInt((s_settings.blessBasa + (param.valueA + param.valueB) * s_settings.blessPerRes) / (float)_blessed.Count);

                    s_humans[param.playerId].Pay(_cost);
                    Cast_Cn(param.playerId, value).Start();
                    _canCast = false;
                }
            }

            private IEnumerator Cast_Cn(int playerId, int value)
            {
                s_isCast.True();

                Actor target;
                for (int i = _blessed.Count - 1, skip; i >= 0; i--)
                {
                    target = _blessed[i];
                    yield return _sfx.Hit(target.Skin);

                    skip = target.Owner != playerId ? 1 : 0;
                    target.AddEffect(new(_attackEffectCode, ActorAbilityId.Attack, TypeModifierId.TotalPercent, value, s_settings.blessDuration, skip));
                    target.AddEffect(new(_defenseEffectCode, ActorAbilityId.Defense, TypeModifierId.TotalPercent, value, s_settings.blessDuration, skip));
                }

                s_isCast.False();
            }
        }
    }
}
