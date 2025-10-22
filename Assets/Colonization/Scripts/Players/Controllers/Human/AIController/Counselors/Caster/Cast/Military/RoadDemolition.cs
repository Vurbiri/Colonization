using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class RoadDemolition : Cast
            {
                private RoadDemolition(Caster parent) : base(parent, MilitarySpellId.Type, MilitarySpellId.RoadDemolition) { }
                public static void Create(Caster parent) => new RoadDemolition(parent);

                public override IEnumerator TryCasting_Cn()
                {
                    DeadEndCount(HumanId, out int friends, out int enemies);
                    if ((friends << 1) < enemies)
                    {
                        yield return CanPayOrExchange_Cn(Out<bool>.Get(out int key));
                        if (Out<bool>.Result(key))
                            yield return Casting_Cn();
                    }

                    // ====== Local ============
                    [Impl(256)] static void DeadEndCount(int playerId, out int friends, out int enemies)
                    {
                        friends = enemies = 0;

                        for (int i = 0; i < PlayerId.HumansCount; i++)
                        {
                            if (GameContainer.Diplomacy.IsHumanEnemy(playerId, i))
                                enemies += GameContainer.Humans[i].Roads.DeadEndCount();
                            else
                                friends += GameContainer.Humans[i].Roads.DeadEndCount();
                        }
                    }
                }
            }
        }
    }
}
