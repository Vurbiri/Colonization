using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed public class Situation : ASituation
        {
            public bool isGuard;
            public int minColonyGuard = int.MaxValue;
            public readonly List<Id<PlayerId>> greatEnemies = new(2);
            public readonly List<Id<PlayerId>> greatFriends = new(3);

            public override void Update(Actor actor)
            {
                FindNearEnemies(actor);

                var hex = actor.Hexagon;
                var crossroads = hex.Crossroads;

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
    }
}
