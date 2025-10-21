using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Zeal : Cast
            {
                private static readonly List<Actor> s_actors = new(CONST.DEFAULT_MAX_WARRIOR);

                private Zeal(Caster parent) : base(parent, MilitarySpellId.Type, MilitarySpellId.Zeal) { }
                public static void Create(Caster parent) => new Zeal(parent);

                public override System.Collections.IEnumerator TryCasting_Cn()
                {
                    if (FindActors(HumanId))
                    {
                        yield return CanPayOrExchange_Cn(OutB.Get(out int key));
                        if (OutB.Result(key))
                        {
                            Human.Pay(Spell.Cost);
                            s_actors.Rand().ZealCharge = true;
                        }
                    }
                    s_actors.Clear();
                    yield break;
                }

                [Impl(256)]
                private static bool FindActors(int playerId)
                {
                    foreach (Actor actor in GameContainer.Actors[playerId])
                        if (!actor.ZealCharge)
                            s_actors.Add(actor);

                    return s_actors.Count > 0;
                }
            }
        }
    }
}
