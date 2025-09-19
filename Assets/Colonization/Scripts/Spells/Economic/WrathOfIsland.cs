using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;
using static Vurbiri.Colonization.CurrencyId;
using static Vurbiri.Colonization.GameContainer;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class WrathOfIsland : ASpell
        {
            private readonly SpellDamager _damage;
            private readonly List<Actor> _targets = new(8);

            private WrathOfIsland(int type, int id) : base(type, id) 
            {
                _damage = new(s_settings.wrathPierce);
                SetManaCost();
            }
            public static void Create() => new WrathOfIsland(EconomicSpellId.Type, EconomicSpellId.Wrath);

            public override bool Prep(SpellParam param)
            {
                if (_canCast = !s_isCast)
                {
                    _targets.Clear();
                    _cost.SetMain(Wood, param.valueA); _cost.SetMain(Ore, param.valueB);

                    if (Humans[param.playerId].IsPay(_cost))
                        FindActorsOnSurface(_targets, SurfaceId.Forest, SurfaceId.Mountain);

                    _canCast = _targets.Count > 0;
                }
                return _canCast;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    int count = _targets.Count;
                    _damage.playerId = param.playerId;
                    _damage.attack = ((s_settings.wrathBasa + (param.valueA + param.valueB) * s_settings.wrathPerRes << ActorAbilityId.SHIFT_ABILITY)) / count;

                    s_isCast.True();

                    Cast_Cn().Start();
                    ShowSpellName(param.playerId, 3f + 2f * count);
                    Humans[param.playerId].Pay(_cost);

                    _canCast = false;
                }
            }

            private IEnumerator Cast_Cn()
            {
                int index; Actor target; Vector3 position = GameContainer.CameraTransform.ParentPosition;
                while (_targets.Count > 0)
                {
                    index = FindNearest(position, _targets);
                    target = _targets[index]; position = target.Position; _targets.RemoveAt(index);

                    CameraController.ToPosition(position, true);
                    _damage.Apply(target);
                    yield return SFX.Run(s_settings.wrathSFX, null, target.Skin);
                }

                s_isCast.False();
            }

            protected override string GetDesc(Localization localization)
            {
                return string.Concat(localization.GetFormatText(FILE, _descKey, s_settings.wrathBasa, s_settings.wrathPerRes, s_settings.wrathPierce), _strCost);
            }
        }
    } 
}
