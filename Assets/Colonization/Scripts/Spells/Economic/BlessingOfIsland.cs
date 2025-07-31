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

            private BlessingOfIsland(int type, int id) : base(type, id) { }
            public static void Create() => new BlessingOfIsland(EconomicSpellId.Type, EconomicSpellId.Blessing);

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

                    s_isCast.True();
                    Cast_Cn(param.playerId, value).Start();
                    
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

                    //yield return GameContainer.CameraController.ToPosition(position);
                    GameContainer.CameraController.ToPosition(position);

                    skip = target.Owner != playerId ? 1 : 0;
                    target.AddEffect(new(_attackEffectCode, ActorAbilityId.Attack, TypeModifierId.TotalPercent, value, s_settings.blessDuration, skip));
                    target.AddEffect(new(_defenseEffectCode, ActorAbilityId.Defense, TypeModifierId.TotalPercent, value, s_settings.blessDuration, skip));

                    yield return GameContainer.HitSFX.Hit(s_settings.blessSFX, s_sfxUser, target.Skin);

                    _blessed.RemoveAt(index);
                }

                s_isCast.False();
            }
        }
    }
}
