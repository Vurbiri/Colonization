using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Order : Cast
            {
                public Order(Caster parent) : base(parent, EconomicSpellId.Type, EconomicSpellId.Order) { }

                public override IEnumerator TryCasting_Cn()
                {
                    int mana = Mana;
                    IEnumerator casting = null;
                    
                    if (Chance.Rolling(100 * mana / s_settings.resDivider))
                        casting = Casting_Cn(Random.Range(1, (int)(mana * s_settings.maxUseRes) + 1));

                    return casting;
                }
            }
        }
    }
}
