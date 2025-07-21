namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class ResourcesMix : ASpell
        {
            private ResourcesMix() { }
            public static void Create() => s_spells[TypeOfPerksId.Economic][EconomicSpellId.ResourcesMix] = new ResourcesMix();

            public override void Cast(SpellParam param, CurrenciesLite resources)
            {
                resources.Mix(s_humans[param.playerId].Resources);
                s_humans[param.playerId].AddResources(resources);
            }

            public override void Clear()
            {
                s_spells[TypeOfPerksId.Economic][EconomicSpellId.ResourcesMix] = null;
            }
        }
    }
}
