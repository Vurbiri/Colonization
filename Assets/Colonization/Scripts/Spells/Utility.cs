using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
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

        private static void FindActorsOnSurface(List<Actor> actors, int surfaceA, int surfaceB)
        {
            for (int i = 0, surface; i < PlayerId.Count; i++)
            {
                foreach (Actor actor in GameContainer.Actors[i])
                {
                    surface = actor.Hexagon.SurfaceId;
                    if (surface == surfaceA | surface == surfaceB)
                        actors.Add(actor);
                }
            }
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
