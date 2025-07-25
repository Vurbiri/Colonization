using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.Characteristics.ReactiveEffectsFactory;
using static Vurbiri.Colonization.CurrencyId;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class WrathOfIsland : ASpell
        {
            private readonly EffectCode _attackEffectCode  = new(SPELL_TYPE, TypeOfPerksId.Economic, WRATH_SKILL_ID, 0);
            private readonly SpellDamager _damage;
            private readonly List<Actor> _targets = new(8);
            private readonly IHitSFX _sfx;

            private WrathOfIsland(IHitSFX sfx, int type, int id) : base(type, id)
            {
                _sfx = sfx;
            }

            public static void Create(IHitSFX sfx) => new WrathOfIsland(sfx, TypeOfPerksId.Economic, EconomicSpellId.Wrath);

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
                    _damage.damage = (s_settings.wrathBasa + (param.valueA + param.valueB) * s_settings.wrathPerRes << ActorAbilityId.SHIFT_ABILITY) / _targets.Count;

                    s_humans[param.playerId].Pay(_cost);
                    Cast_Cn().Start();
                    _canCast = false;
                }
            }

            private IEnumerator Cast_Cn()
            {
                s_isCast.True();

                for (int i = _targets.Count - 1; i >= 0; i--)
                    yield return _damage.Apply_Cn(_targets[i]);

                s_isCast.False();
            }
        }

        sealed private class SpellDamager : Effect
        {
            private readonly IHitSFX _sfx;
            private readonly AbilityModifierPercent _pierce;
            
            public int damage, playerId;

            public SpellDamager(IHitSFX sfx) : base(ActorAbilityId.CurrentHP, TypeModifierId.Addition, 0)
            {
                _sfx = sfx;
                _pierce = new(100 - s_settings.wrathPierce);
            }

            public IEnumerator Apply_Cn(Actor target)
            {
                _value = -Formulas.Damage(damage, _pierce.Apply(target.Abilities[ActorAbilityId.Defense].Value));

                yield return _sfx.Hit(target.Skin);
                target.ApplyEffect(this);

                if (target.IsDead & target.Owner != playerId)
                    GameContainer.TriggerBus.TriggerActorKill(playerId, target.TypeId, target.Id);
            }
        }
    }
}
