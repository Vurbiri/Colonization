namespace Vurbiri.Colonization.Characteristics
{
    public abstract class PerkModifierId : IdType<PerkModifierId>
    {
        public const int Enable  = 0;
        public const int Flat    = 1;
        public const int Percent = 2;

        static PerkModifierId() => ConstructorRun();

        public static int ToTypeModifier(int modifier) => modifier switch
        {
            Enable  => TypeModifierId.Addition,
            Flat    => TypeModifierId.Addition,
            Percent => TypeModifierId.BasePercent,
            _       => Errors.ArgumentOutOfRange("PerkModifierId", modifier),
        };
    }
}
