using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI
        {
            #region ************* ASituation **********************
            public abstract class ASituation
            {
                public bool isInCombat;
                public readonly List<ActorCode> nearEnemies = new(3);

                public abstract void Update(Actor actor);

                [Impl(256)]
                protected void FindNearEnemies(Actor actor)
                {
                    var near = actor._currentHex.Neighbors;

                    nearEnemies.Clear();
                    for (int i = 0; i < HEX.SIDES; i++)
                        if (near[i].TryGetEnemy(actor._owner, out Actor enemy))
                            nearEnemies.Add(enemy);
                    isInCombat = nearEnemies.Count > 0;
                }
            }
            #endregion

            #region ************* ActorData **********************
            public readonly struct ActorData : System.IEquatable<ActorData>, System.IEquatable<ActorCode>
            {
                public readonly ActorCode code;
                public readonly int force;

                public ActorData(Actor actor)
                {
                    code = actor.Code;
                    force = actor.CurrentForce;
                }
                public ActorData(ActorCode code)
                {
                    this.code = code;
                    force = 0;
                }

                public readonly bool Equals(ActorData other) => code == other.code;
                public readonly bool Equals(ActorCode actor) => code == actor;
                public readonly override bool Equals(object obj) => obj is not null && ((obj is ActorData other && code == other.code) || (obj is ActorCode actor && code == actor));

                public readonly override int GetHashCode() => code.GetHashCode();
            }
            #endregion
        }
    }
}
