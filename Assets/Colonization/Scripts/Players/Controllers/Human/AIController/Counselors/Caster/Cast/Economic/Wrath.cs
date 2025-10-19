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
            sealed private class Wrath : Cast
            {
                public Wrath(Caster parent) : base(parent, EconomicSpellId.Type, EconomicSpellId.Wrath) { }

                public override IEnumerator TryCasting_Cn()
                {
                    int wood = Resources[CurrencyId.Wood], ore = Resources[CurrencyId.Ore];

                    yield return CanPay_Cn(OutB.Get(out int key));

                    if (OutB.Result(key) && Chance.Rolling((100 * (wood + ore)/(s_settings.resDivider << 1))))
                    {
                        FindActors(out int friends, out int enemies);
                        if ((friends << 1) < enemies)
                            yield return Casting_Cn(GetRes(wood), GetRes(ore));
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
                                if (surface == SurfaceId.Forest | surface == SurfaceId.Mountain)
                                {
                                    if (actor.IsEnemy(HumanId))
                                        enemies++;
                                    else
                                        friends++;
                                }
                            }
                        }
                    }
                    //===========================================
                    [Impl(256)] static int GetRes(int res)
                    {
                        int result = 0;
                        if(res > 0)
                            Random.Range(1, Mathf.Min(res, s_settings.maxUseRes) + 1);
                        return result;
                    }
                    #endregion
                }
            }
        }
    }
}
