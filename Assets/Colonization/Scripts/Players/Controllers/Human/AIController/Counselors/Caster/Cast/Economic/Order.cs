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
                private Order(Caster parent) : base(parent, EconomicSpellId.Type, EconomicSpellId.Order) { }
                public static void Create(Caster parent) => new Order(parent);

                public override IEnumerator TryCasting_Cn()
                {
                    int mana = (int)(Mana * s_settings.useResRatio);
                    return Chance.Rolling(10 * mana) ? Casting_Cn(Random.Range(1, mana + 1)) : null;
                }
            }
        }
    }
}
