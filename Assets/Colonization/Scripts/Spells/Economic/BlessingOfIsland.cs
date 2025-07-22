using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.Characteristics.ReactiveEffectsFactory;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class BlessingOfIsland : ASpell
        {
            private readonly EffectCode _attackEffectCode  = new(SPELL_TYPE, TypeOfPerksId.Economic, BLESS_SKILL_ID, 0);
            private readonly EffectCode _defenseEffectCode = new(SPELL_TYPE, TypeOfPerksId.Economic, BLESS_SKILL_ID, 1);
            private readonly List<Actor> _blessed = new(8);
            private readonly IHitSFX _sfx;

            private BlessingOfIsland(IHitSFX sfx)
            {
                _sfx = sfx;
            }

            public static void Create(IHitSFX sfx) => s_spells[TypeOfPerksId.Economic][EconomicSpellId.Blessing] = new BlessingOfIsland(sfx);

            public override void Cast(SpellParam param, CurrenciesLite resources)
            {
                _blessed.Clear();
                for (int i = 0, surface; i < PlayerId.HumansCount; i++)
                {
                    foreach (Actor actor in s_actors[i])
                    {
                        surface = actor.Hexagon.SurfaceId;
                        if (surface == SurfaceId.Village | surface == SurfaceId.Field)
                            _blessed.Add(actor);
                    }
                }
                if (_blessed.Count > 0)
                {
                    int value = Mathf.RoundToInt((s_settings.blessBasa + (param.valueA + param.valueB) * s_settings.blessPerRes) / (float)_blessed.Count);
                    Cast_Cn(param.playerId, value).Start();
                    
                    resources.Add(CurrencyId.Gold, -param.valueA); resources.Add(CurrencyId.Food, -param.valueB);
                    s_humans[param.playerId].AddResources(resources);
                }
            }

            public override void Clear()
            {
                s_spells[TypeOfPerksId.Economic][EconomicSpellId.Blessing] = null;
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
                    target.AddEffect(new(_attackEffectCode,  ActorAbilityId.Attack,  TypeModifierId.TotalPercent, value, s_settings.blessDuration, skip));
                    target.AddEffect(new(_defenseEffectCode, ActorAbilityId.Defense, TypeModifierId.TotalPercent, value, s_settings.blessDuration, skip));
                }

                s_isCast.False();
            }
        }
    }
}
