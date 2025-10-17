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
                public Order(Caster parent) : base(parent, EconomicSpellId.Type, EconomicSpellId.Order)
                {
                }

                public override IEnumerator Casting_Cn()
                {
                    IEnumerator casting = null;
                    int mana = Resources[CurrencyId.Mana];
                    if (Chance.Rolling(100 * mana / s_settings.maxMana))
                        casting = Casting_Cn(new(PlayerId, Random.Range(1, Mathf.Min(mana, s_settings.maxMana) + 1)));

                    return casting;
                }
            }
        }
    }
}
