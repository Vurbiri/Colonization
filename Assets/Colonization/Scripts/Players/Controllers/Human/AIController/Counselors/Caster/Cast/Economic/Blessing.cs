using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Blessing : Cast
            {
                private Blessing(Caster parent) : base(parent, EconomicSpellId.Type, EconomicSpellId.Blessing) { }
                public static void Create(Caster parent) => new Blessing(parent);

                public override IEnumerator TryCasting_Cn()
                {
                    IEnumerator casting = null;
                    if (CanPay)
                    { 
                        FindActors(HumanId, out int friends, out int enemies);
                        if (friends > enemies)
                            casting = Casting_Cn(GetRes(CurrencyId.Gold), GetRes(CurrencyId.Food));
                    }

                    return casting;

                    #region Local FindActors(..), GetRes(..)
                    //===========================================
                    static void FindActors(int playerId, out int friends, out int enemies)
                    {
                        friends = enemies = 0;

                        bool isEnemy;
                        for (int i = 0, surface; i < PlayerId.Count; i++)
                        {
                            isEnemy = GameContainer.Diplomacy.IsEnemy(playerId, i);
                            foreach (Actor actor in GameContainer.Actors[i])
                            {
                                surface = actor.Hexagon.SurfaceId;
                                if (surface == SurfaceId.Village | surface == SurfaceId.Field)
                                {
                                    if (isEnemy)
                                        enemies++;
                                    else if (actor.IsInCombat())
                                        friends++;
                                }
                            }
                        }
                    }
                    //===========================================
                    [Impl(256)] int GetRes(int id) => Random.Range(0, (int)(Resources[id] * s_settings.useResRatio) + 1);
                    #endregion
                }
            }
        }
    }
}
