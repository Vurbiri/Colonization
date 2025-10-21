using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Transmutation : Cast
            {
                private Transmutation(Caster parent) : base(parent, EconomicSpellId.Type, EconomicSpellId.Transmutation) { }
                public static void Create(Caster parent) => new Transmutation(parent);

                public override IEnumerator TryCasting_Cn()
                {
                    return CanPay && CheckResources(Resources) ? Casting_Cn() : null;

                    // ====== Local ======
                    static bool CheckResources(Currencies resources)
                    {
                        int count = 0, currency; bool isMin = false;
                        for (int i = 0; i < CurrencyId.MainCount - 1; i++)
                        {
                            currency = resources[i];
                            count += 100 * currency;
                            isMin |= currency <= s_settings.minRes;
                        }

                        return isMin && Chance.Rolling(count / resources.MaxAmount);
                    }
                }
            }
        }
    }
}
