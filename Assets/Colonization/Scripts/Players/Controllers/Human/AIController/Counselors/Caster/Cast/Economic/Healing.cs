using System.Collections;
using Vurbiri.Colonization.Actors;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Healing : Cast
            {
                public Healing(Caster parent) : base(parent, EconomicSpellId.Type, EconomicSpellId.RandomHealing) { }

                public override IEnumerator TryCasting_Cn()
                {
                    FindActors(HumanId, out int friends, out int enemies);
                    if (friends > (enemies << 1))
                    {
                        yield return CanPayOrExchange_Cn(OutB.Get(out int key));
                        if (OutB.Result(key))
                            yield return Casting_Cn();
                    }

                    // ====== Local ============
                    [Impl(256)] static void FindActors(int playerId, out int friends, out int enemies)
                    {
                        friends = enemies = 0;

                        for (int i = 0; i < PlayerId.Count; i++)
                        {
                            foreach (Actor actor in GameContainer.Actors[i])
                            {
                                if (actor.IsWounded)
                                {
                                    if (actor.IsEnemy(playerId))
                                        enemies++;
                                    else
                                        friends++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
