using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Blessing : Cast
            {
                public Blessing(Caster parent) : base(parent, EconomicSpellId.Type, EconomicSpellId.Blessing) { }

                public override IEnumerator TryCasting_Cn()
                {
                    yield return CanPay_Cn(OutB.Get(out int key));
                    if (OutB.Result(key))
                    {
                        FindActors(out int friends, out int enemies);
                        if (friends > enemies)
                            yield return Casting_Cn(GetRes(CurrencyId.Gold), GetRes(CurrencyId.Food));
                    }

                    #region Local FindActors(..), GetRes(..)
                    //===========================================
                    [Impl(256)] void FindActors(out int friends, out int enemies)
                    {
                        friends = enemies = 0;

                        for (int i = 0, surface; i < PlayerId.Count; i++)
                        {
                            foreach (Actor actor in GameContainer.Actors[i])
                            {
                                surface = actor.Hexagon.SurfaceId;
                                if (surface == SurfaceId.Village | surface == SurfaceId.Field)
                                {
                                    if (actor.IsEnemy(HumanId))
                                        enemies++;
                                    else if (actor.IsInCombat())
                                        friends++;
                                }
                            }
                        }
                    }
                    //===========================================
                    [Impl(256)] int GetRes(int id) => Random.Range(0, Mathf.Min(Resources[id], s_settings.maxUseRes) + 1);
                    #endregion
                }
            }
        }
    }
}
