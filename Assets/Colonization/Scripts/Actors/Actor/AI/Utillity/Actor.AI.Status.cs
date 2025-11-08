using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI
        {
            public abstract class AStatus
            {
                public int percentHP;
                public bool isInCombat, isMove, isSiege, isGuard;
                public int forceNearTwoEnemies;

                public readonly WeightsList<Actor> nearEnemies = new(null, 3);

                public readonly List<Crossroad> ownerColonies = new(3);
                public readonly List<Crossroad> enemyColonies = new(3);

                public readonly List<Id<PlayerId>> enemies = new(3);

                public virtual void Update(Actor actor)
                {
                    percentHP = actor.PercentHP;
                    isMove = actor.Action.CanUsedMoveSkill();

                    SetNearEnemies(actor);
                    SetForceNearTwoEnemy(actor);
                    SetColonies(actor);

                    for (int i = 0; i < PlayerId.Count; i++)
                        if (GameContainer.Diplomacy.IsEnemy(actor._owner, i))
                            enemies.Add(i);
                }

                public void Clear()
                {
                    nearEnemies.Clear();
                    ownerColonies.Clear();
                    enemyColonies.Clear();
                    enemies.Clear();

                }

                [Impl(256)] public bool CanMoveToEnemy(int minHP) => isMove & percentHP > minHP;

                [Impl(256)] private void SetNearEnemies(Actor actor)
                {
                    var near = actor._currentHex.Neighbors;
                    for (int i = 0; i < HEX.SIDES; i++)
                        if (near[i].TryGetEnemy(actor._owner, out Actor enemy))
                            nearEnemies.Add(enemy, GameContainer.Actors.MaxForce - enemy.CurrentForce);

                    isInCombat = nearEnemies.Count > 0;
                }

                [Impl(256)] private void SetColonies(Actor actor)
                {
                    if (!isInCombat)
                    {
                        var id = actor._owner;
                        var crossroads = actor._currentHex.Crossroads;
                        for (int i = 0; i < HEX.VERTICES; i++)
                        {
                            if (crossroads[i].TryGetOwnerColony(out Id<PlayerId> owner))
                            {
                                if (id == owner)
                                    ownerColonies.Add(crossroads[i]);
                                else if (GameContainer.Diplomacy.IsEnemy(id, owner))
                                    enemyColonies.Add(crossroads[i]);
                            }
                        }
                    }
                    isGuard = ownerColonies.Count > 0;
                    isSiege = enemyColonies.Count > 0;
                }

                [Impl(256)] private void SetForceNearTwoEnemy(Actor actor)
                {
                    forceNearTwoEnemies = 0;
                    var playerId = actor._owner;
                    Key current = actor._currentHex.Key;

                    for (int i = 0; i < HEX.NEAR_TWO.Count; i++)
                        if (GameContainer.Hexagons.TryGet(current + HEX.NEAR_TWO[i], out Hexagon hex) && hex.TryGetEnemy(playerId, out Actor enemy))
                            forceNearTwoEnemies += enemy.CurrentForce;
                }
            }
        }
    }
}
