namespace Vurbiri.Colonization
{
    public class EconomicPerksId : APerkId<EconomicPerksId>
    {
        public const int MaxResources_1 = 0;
        public const int MaxResources_2 = 1;
        public const int MaxResources_3 = 2;

        static EconomicPerksId() => RunConstructor();
        private EconomicPerksId() { }
    }
}
