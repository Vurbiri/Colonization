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
                private Wrath(Caster parent) : base(parent, EconomicSpellId.Type, EconomicSpellId.Wrath) { }
                public static void Create(Caster parent) => new Wrath(parent);

                public override IEnumerator TryCasting_Cn()
                {
                    IEnumerator casting = null;
                    if (CanPay)
                    {
                        int wood = Resources[CurrencyId.Wood], ore = Resources[CurrencyId.Ore];
                        if (Chance.Rolling((100 * (wood + ore) / (s_settings.resDivider << 1))))
                        {
                            FindActors(HumanId, out int friends, out int enemies);
                            if ((friends << 1) < enemies)
                                casting = Casting_Cn(GetRes(wood), GetRes(ore));
                        }
                    }
                    return casting;

                    #region Local FindActors(..), GetRes(..)
                    //===========================================
                    [Impl(256)] static void FindActors(int playerId, out int friends, out int enemies)
                    {
                        friends = enemies = 0;

                        for (int i = 0, surface; i < PlayerId.Count; i++)
                        {
                            foreach (Actor actor in GameContainer.Actors[i])
                            {
                                surface = actor.Hexagon.SurfaceId;
                                if (surface == SurfaceId.Forest | surface == SurfaceId.Mountain)
                                {
                                    if (actor.IsEnemy(playerId))
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
                            Random.Range(1, (int)(res * s_settings.maxUseRes) + 1);
                        return result;
                    }
                    #endregion
                }
            }
        }
    }
}
