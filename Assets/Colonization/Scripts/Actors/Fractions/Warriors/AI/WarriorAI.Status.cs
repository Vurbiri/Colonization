namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed public class Status : AStatus
        {
            public int minColonyGuard;

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
            }
        }
    }
}
