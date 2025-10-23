using System.Collections;
using UnityEngine;
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
                        int wood = (int)(Resources[CurrencyId.Wood] * s_settings.useResRatio), ore = (int)(Resources[CurrencyId.Ore] * s_settings.useResRatio);
                        if (Chance.Rolling((5 * (wood + ore))))
                        {
                            FindActors(HumanId, out int friends, out int enemies);
                            if ((friends << 1) < enemies)
                                casting = Casting_Cn(GetRes(wood), GetRes(ore));
                        }
                    }
                    return casting;

                    #region Local FindActors(..), GetRes(..)
                    //===========================================
                    static void FindActors(int playerId, out int friends, out int enemies)
                    {
                        friends = enemies = 0;

                        for (int i = 0, surface, count; i < PlayerId.Count; i++)
                        {
                            count = 0;
                            foreach (Actor actor in GameContainer.Actors[i])
                            {
                                surface = actor.Hexagon.SurfaceId;
                                if (surface == SurfaceId.Forest | surface == SurfaceId.Mountain)
                                    count++;
                            }
                            if (GameContainer.Diplomacy.IsEnemy(playerId, i))
                                enemies += count;
                            else
                                friends += count;
                        }
                    }
                    //===========================================
                    [Impl(256)] static int GetRes(int res)
                    {
                        if(res > 0) res = Random.Range(1, res + 1);
                        return res;
                    }
                    #endregion
                }
            }
        }
    }
}
