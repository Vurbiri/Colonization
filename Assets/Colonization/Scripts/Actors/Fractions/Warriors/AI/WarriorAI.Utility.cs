using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        #region ************* Situation **********************
        private class Situation
        {
            public bool isInCombat;
            public readonly List<ActorCode> enemies;
            public bool isGuard;
            public int minColonyGuard;
            public readonly List<Id<PlayerId>> greatEnemies;
            public readonly List<Id<PlayerId>> greatFriends;

            public Situation()
            {
                minColonyGuard = int.MaxValue;
                enemies = new(3);
                greatEnemies = new(2);
                greatFriends = new(3);
            }

            public void Update(Actor actor)
            {
                var hex = actor.Hexagon;
                var near = hex.Neighbors;
                var crossroads = hex.Crossroads;

                enemies.Clear();
                for (int i = 0; i < HEX.SIDES; i++)
                    if (near[i].TryGetEnemy(actor.Owner, out Actor enemy))
                        enemies.Add(enemy);
                isInCombat = enemies.Count > 0;

                isGuard = false;
                minColonyGuard = int.MaxValue;
                for (int i = 0, count; i < HEX.VERTICES; i++)
                {
                    count = crossroads[i].GetGuardCount(actor.Owner);
                    if (count > 0)
                    {
                        isGuard = true;
                        minColonyGuard = System.Math.Min(minColonyGuard, count);
                    }
                }

                greatEnemies.Clear(); greatFriends.Clear();
                for (int i = 0; i < PlayerId.HumansCount; i++)
                {
                    if (GameContainer.Diplomacy.IsGreatEnemy(actor.Owner, i))
                        greatEnemies.Add(i);
                    else if(i != actor.Owner && GameContainer.Diplomacy.IsGreatFriend(actor.Owner, i))
                        greatFriends.Add(i);
                }
                greatFriends.Add(actor.Owner);
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
