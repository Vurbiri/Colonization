using System.Collections;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Healing : Cast
            {
                private Healing(Caster parent) : base(parent, EconomicSpellId.Type, EconomicSpellId.RandomHealing) { }
                public static void Create(Caster parent) => new Healing(parent);

                public override IEnumerator TryCasting_Cn()
                {
                    FindActors(HumanId, out int friends, out int enemies);
                    if (friends > (enemies << 1))
                    {
                        yield return CanPayOrExchange_Cn(Out<bool>.Get(out int key));
                        if (Out<bool>.Result(key))
                            yield return Casting_Cn();
                    }

                    // ====== Local ============
                    static void FindActors(int playerId, out int friends, out int enemies)
                    {
                        friends = enemies = 0;

                        for (int i = 0, count = 0; i < PlayerId.Count; i++, count = 0)
                        {
                            foreach (Actor actor in GameContainer.Actors[i])
                                if (actor.IsWounded) count++;

                            if (GameContainer.Diplomacy.IsEnemy(playerId, i))
                                enemies += count;
                            else
                                friends += count;
                        }
                    }
                }
            }
        }
    }
}
