using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
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
            }
            public static void Create() => new WrathOfIsland(EconomicSpellId.Type, EconomicSpellId.Wrath);

            public override bool Prep(SpellParam param)
            {
                _targets.Clear();
                _cost.Set(Wood, param.valueA); _cost.Set(Ore, param.valueB);

                if (s_humans[param.playerId].IsPay(_cost))
                {
                    for (int i = 0, surface; i < PlayerId.HumansCount; i++)
                    {
                        foreach (Actor actor in s_actors[i])
                        {
                            surface = actor.Hexagon.SurfaceId;
                            if (surface == SurfaceId.Forest | surface == SurfaceId.Mountain)
                                _targets.Add(actor);
                        }
                    }
                }
                return _canCast = _targets.Count > 0;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    _damage.playerId = param.playerId;
                    _damage.attack = (s_settings.wrathBasa + (param.valueA + param.valueB) * s_settings.wrathPerRes << ActorAbilityId.SHIFT_ABILITY) / _targets.Count;

                    s_humans[param.playerId].Pay(_cost);

                    s_isCast.True();
                    Cast_Cn().Start();

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
                    yield return HitSFX.Hit(s_settings.wrathSFX, s_sfxUser, target.Skin);
                }

                s_isCast.False();
            }
        }
    } 
}
