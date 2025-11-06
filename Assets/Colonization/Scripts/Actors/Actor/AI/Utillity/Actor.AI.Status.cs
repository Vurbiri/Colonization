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
                public bool isMove;
                public bool isInCombat;
                public readonly List<ActorCode> nearEnemies = new(3);

                public virtual void Update(Actor actor)
                {
                    percentHP = actor.PercentHP;
                    isMove = actor.Action.CanUseMoveSkill();

                    var near = actor._currentHex.Neighbors;
                    nearEnemies.Clear();
                    for (int i = 0; i < HEX.SIDES; i++)
                        if (near[i].TryGetEnemy(actor._owner, out Actor enemy))
                            nearEnemies.Add(enemy);
                    isInCombat = nearEnemies.Count > 0;
                }

                [Impl(256)] public bool CanMove(int minHP) => isMove && percentHP > minHP;
            }
        }
    }
}
