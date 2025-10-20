using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class BloodTrade : Cast
            {
                public BloodTrade(Caster parent) : base(parent, MilitarySpellId.Type, MilitarySpellId.BloodTrade) { }

                public override IEnumerator TryCasting_Cn()
                {
                    IEnumerator casting = null;
                    int blood = Resources[CurrencyId.Blood];

                    if (Resources.PercentAmount < 100 & PerkTree.IsAllLearned() & blood >= SpellBook.BloodTradeCost)
                        casting = Casting_Cn(Random.Range(SpellBook.BloodTradeCost, blood + 1));

                    return casting;
                }
            }
        }
    }
}
