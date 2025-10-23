namespace Vurbiri.Colonization
{
    public class AbilityValue : IAbilityValue
    {
        public Id<TypeModifierId> TypeModifier { get; }
        public int Value { get; }

        public AbilityValue(Id<TypeModifierId> type, int value)
        {
            TypeModifier = type;
            Value = value;
        }
    }
}
