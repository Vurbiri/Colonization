using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        private class Status
        {
            public bool isInCombat;
            public readonly List<ActorCode> enemies;
            public bool isGuard;
            public int minGuard;

            public Status()
            {
                minGuard = int.MaxValue;
                enemies = new(3);
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

                minGuard = int.MaxValue;
                for (int i = 0, count; i < HEX.VERTICES; i++)
                {
                    count = crossroads[i].GetGuardCount(actor.Owner);
                    if (count > 0)
                    {
                        isGuard = true;
                        minGuard = System.Math.Min(minGuard, count);
                    }
                }
            }
        }
    }
}
