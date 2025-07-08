using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.Characteristics.ReactiveEffectsFactory;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class WrathOfIsland : ASharedSpell
        {
            private readonly EffectCode _attackEffectCode  = new(SPELL_TYPE, TypeOfPerksId.Economic, WRATH_SKILL_ID, 0);
            private readonly SpellDamager _damage;
            private readonly List<Actor> _targets = new(8);
            private readonly IHitSFX _sfx;

            private WrathOfIsland(IHitSFX sfx)
            {
                _damage = new (sfx);
            }

            public static void Create(IHitSFX sfx) => s_sharedSpells[TypeOfPerksId.Economic][EconomicSpellId.Wrath] = new WrathOfIsland(sfx);

            public override bool Cast(SpellParam param, CurrenciesLite resources)
            {
                _targets.Clear();
                for (int i = 0, surface; i < PlayerId.HumansCount; i++)
                {
                    foreach (Actor actor in s_actors[i])
                    {
                        surface = actor.Hexagon.SurfaceId;
                        if (surface == SurfaceId.Forest | surface == SurfaceId.Mountain)
                            _targets.Add(actor);
                    }
                }
                if (_targets.Count > 0)
                {
                    _damage.playerId = param.playerId;
                    _damage.damage = (s_settings.wrathBasa + (param.valueA + param.valueB) * s_settings.wrathPerRes << ActorAbilityId.SHIFT_ABILITY) / _targets.Count;

                    s_coroutines.Run(Cast_Cn());

                    resources.Add(CurrencyId.Wood, -param.valueA); resources.Add(CurrencyId.Ore, -param.valueB);
                    return true;
                }

                return false;
            }

            private IEnumerator Cast_Cn()
            {
                for (int i = _targets.Count - 1; i >= 0; i--)
                    yield return _damage.Apply_Cn(_targets[i]);
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
                    s_triggerBus.TriggerActorKill(playerId, target.TypeId, target.Id);
            }
        }
    }
}
