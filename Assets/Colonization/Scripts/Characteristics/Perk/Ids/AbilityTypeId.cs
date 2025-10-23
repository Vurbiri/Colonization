using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public abstract class AbilityTypeId : IdType<AbilityTypeId>
    {
        public const int Economic = 0;
        public const int Military = 1;

        [NotId] public static readonly ReadOnlyArray<int> PerksCount;
        [NotId] public static readonly ReadOnlyArray<int> SpellsCount;

        public static int Other(int type) => 1 - type;

        static AbilityTypeId()
        {
            ConstructorRun();

            int[] perks = { EconomicPerksId.Count, MilitaryPerksId.Count };
            PerksCount = perks;
            int[] spells = { EconomicSpellId.Count, MilitarySpellId.Count };
            SpellsCount = spells;
        }
    }
}
