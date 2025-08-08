using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;
using static Vurbiri.Colonization.Characteristics.ReactiveEffectsFactory;
using static Vurbiri.Colonization.CurrencyId;
using static Vurbiri.Colonization.GameContainer;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class BlessingOfIsland : ASpell
        {
            private readonly EffectCode _attackEffectCode = new(SPELL_TYPE, EconomicSpellId.Type, BLESS_SKILL_ID, 0);
            private readonly EffectCode _defenseEffectCode = new(SPELL_TYPE, EconomicSpellId.Type, BLESS_SKILL_ID, 1);
            private readonly List<Actor> _blessed = new(8);

            private BlessingOfIsland(int type, int id) : base(type, id) => SetManaCost();
            public static void Create() => new BlessingOfIsland(EconomicSpellId.Type, EconomicSpellId.Blessing);

            public override bool Prep(SpellParam param)
            {
                _blessed.Clear();
                _cost.Set(Gold, param.valueA); _cost.Set(Food, param.valueB);

                if (!s_isCast && s_humans[param.playerId].IsPay(_cost))
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
                    float count = _blessed.Count;
                    int value = Mathf.RoundToInt((s_settings.blessBasa + (param.valueA + param.valueB) * s_settings.blessPerRes) / count);

                    s_isCast.True();

                    Cast_Cn(param.playerId, value).Start();
                    ShowNameSpell(param.playerId, 3f + 2f * count);
                    s_humans[param.playerId].Pay(_cost);

                    _canCast = false;
                }
            }

            private IEnumerator Cast_Cn(int playerId, int value)
            {
                int index, skip;  Actor target; Vector3 position = GameContainer.CameraTransform.ParentPosition;
                while (_blessed.Count > 0)
                {
                    index = FindNearest(position, _blessed);
                    target = _blessed[index]; position = target.Position;

                    CameraController.ToPosition(position, true);

                    skip = target.Owner != playerId ? 1 : 0;
                    target.AddEffect(new(_attackEffectCode, ActorAbilityId.Attack, TypeModifierId.TotalPercent, value, s_settings.blessDuration, skip));
                    target.AddEffect(new(_defenseEffectCode, ActorAbilityId.Defense, TypeModifierId.TotalPercent, value, s_settings.blessDuration, skip));

                    yield return HitSFX.Hit(s_settings.blessSFX, s_sfxUser, target.Skin);

                    _blessed.RemoveAt(index);
                }

                s_isCast.False();
            }

            protected override string GetDesc(Localization localization)
            {
                return string.Concat(localization.GetFormatText(FILE, _descKey, s_settings.blessBasa, s_settings.blessPerRes, s_settings.blessDuration), _strCost);
            }
        }
    }
}
