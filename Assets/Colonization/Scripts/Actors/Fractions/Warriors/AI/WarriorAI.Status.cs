using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed public class Status : AStatus
        {
            public int minColonyGuard;
            public readonly List<Id<PlayerId>> greatEnemies = new(2);
            public readonly List<Id<PlayerId>> greatFriends = new(3);

            public override void Update(Actor actor)
            {
                base.Update(actor);

                minColonyGuard = int.MaxValue;
                if (isGuard)
                {
                    for (int i = ownerColonies.Count - 1, count; i >= 0; i--)
                    {
                        count = ownerColonies[i].GetOwnerCount(actor.Owner);
                        if (count > 0)
                            minColonyGuard = System.Math.Min(minColonyGuard, count);
                    }
                }

                for (int i = 0; i < PlayerId.HumansCount; i++)
                {
                    if (GameContainer.Diplomacy.IsGreatEnemy(actor.Owner, i))
                        greatEnemies.Add(i);
                    else if(i != actor.Owner && GameContainer.Diplomacy.IsGreatFriend(actor.Owner, i))
                        greatFriends.Add(i);
                }
                greatFriends.Add(actor.Owner);
            }

            public override void Clear()
            {
                base.Clear();

                greatEnemies.Clear(); 
                greatFriends.Clear();
            }
        }
    }
}
