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
                public readonly WeightsList<Actor> nearEnemies = new(null, 3);
                public readonly List<Crossroad> ownerColonies = new(3);
                public readonly List<Crossroad> enemyColonies = new(3);

                public virtual void Update(Actor actor)
                {
                    percentHP = actor.PercentHP;
                    isMove = actor.Action.CanUseMoveSkill();

                    var near = actor._currentHex.Neighbors;
                    for (int i = 0; i < HEX.SIDES; i++)
                        if (near[i].TryGetEnemy(actor._owner, out Actor enemy))
                            nearEnemies.Add(enemy, GameContainer.Actors.MaxForce - enemy.CurrentForce);
                    
                    isInCombat = nearEnemies.Count > 0;

                    if(!isInCombat)
                    {
                        var crossroads = actor._currentHex.Crossroads;
                        for (int i = 0; i < HEX.VERTICES; i++)
                        {
                            if (crossroads[i].TryGetOwnerColony(out Id<PlayerId> owner))
                            {
                                if (actor._owner == owner)
                                    ownerColonies.Add(crossroads[i]);
                                else if (GameContainer.Diplomacy.IsEnemy(actor._owner, owner))
                                    enemyColonies.Add(crossroads[i]);
                            }
                        }
                    }
                    isGuard = ownerColonies.Count > 0;
                    isSiege = enemyColonies.Count > 0;
                }

                public virtual void Clear()
                {
                    nearEnemies.Clear();
                    ownerColonies.Clear();
                    enemyColonies.Clear();
                }

                [Impl(256)] public bool CanMove(int minHP) => isMove & percentHP > minHP;
            }
        }
    }
}
