using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;
using static Vurbiri.Colonization.CONST;
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
                if (_canCast = !s_isCasting)
                {
                    _blessed.Clear();
                    _cost[Gold] = param.valueA; _cost[Food] = param.valueB;

                    if (Humans[param.playerId].IsPay(_cost))
                        FindActorsOnSurface(_blessed, SurfaceId.Village, SurfaceId.Field);

                    _canCast = _blessed.Count > 0;
                }
                return _canCast;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    float count = _blessed.Count;
                    int value = MathI.Round((s_settings.blessBasa + (param.valueA + param.valueB) * s_settings.blessPerRes) / count);

                    StartCasting();

                    Cast_Cn(param.playerId, value).Start();
                    ShowSpellName(param.playerId, 3f + 2f * count);
                    Humans[param.playerId].Pay(_cost);

                    _canCast = false;
                }
            }

            private IEnumerator Cast_Cn(int playerId, int value)
            {
                int index, skip;  Actor target; Vector3 position = CameraTransform.ParentPosition;
                while (_blessed.Count > 0)
                {
                    index = FindNearest(position, _blessed);
                    target = _blessed[index]; position = target.Position;

                    CameraController.ToPosition(position, true);

                    skip = target.Owner != playerId ? 1 : 0;
                    target.AddEffect(new(_attackEffectCode, ActorAbilityId.Attack, TypeModifierId.TotalPercent, value, s_settings.blessDuration, skip));
                    target.AddEffect(new(_defenseEffectCode, ActorAbilityId.Defense, TypeModifierId.TotalPercent, value, s_settings.blessDuration, skip));

                    yield return SFX.Run(s_settings.blessSFX, null, target.Skin);

                    _blessed.RemoveAt(index);
                }

                EndCasting();
            }

            protected override string GetDesc(Localization localization)
            {
                return string.Concat(localization.GetFormatText(FILE, _descKey, s_settings.blessBasa, s_settings.blessPerRes, s_settings.blessDuration), _strCost);
            }
        }
    }
}
