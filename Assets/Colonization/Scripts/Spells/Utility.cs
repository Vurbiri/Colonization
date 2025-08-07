using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{

    public class SpellCosts : ReadOnlyCollection<ReadOnlyCollection<int>>
    {
        public SpellCosts(ReadOnlyCollection<int> e, ReadOnlyCollection<int> m) : base(new ReadOnlyCollection<int>[] { e, m }) { }
    }
    public class SpellKeys : ReadOnlyCollection<ReadOnlyCollection<string>>
    {
        public SpellKeys(ReadOnlyCollection<string> e, ReadOnlyCollection<string> m) : base(new ReadOnlyCollection<string>[] { e, m }) { }
    }

    public partial class SpellBook
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int FindNearest(Vector3 point, List<Actor> actors)
        {
            int output = 0;
            float minDistance = float.MaxValue, curDistance;
            for (int index = actors.Count - 1; index >= 0; index--)
            {
                curDistance = point.SqrDistance(actors[index].Position);
                if (curDistance < minDistance )
                {
                    minDistance = curDistance;
                    output = index;
                }
            }
            return output;
        }

        sealed private class SpellDamager : Effect
        {
            private readonly AbilityModifierPercent _pierce;

            public int attack, playerId;

            public SpellDamager(int pierce) : base(ActorAbilityId.CurrentHP, TypeModifierId.Addition, 0)
            {
                _pierce = new(100 - pierce);
            }

            public void Apply(Actor target)
            {
                _value = -Formulas.Damage(attack, _pierce.Apply(target.Abilities[ActorAbilityId.Defense].Value));

                target.ApplyEffect(this);
                if (target.IsDead & target.Owner != playerId)
                    GameContainer.TriggerBus.TriggerActorKill(playerId, target.TypeId, target.Id);
            }
        }
    }
}
