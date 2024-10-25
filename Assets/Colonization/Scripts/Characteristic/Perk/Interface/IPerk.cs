namespace Vurbiri.Colonization
{
    public interface IPerk<T> where T : AStateId<T>
    {
        public int Id { get; }
        public int Level { get; }
        public Id<TargetOfPerkId> TargetObject { get; }
        public Id<T> TargetAbility { get; }
        public CurrenciesLite Cost { get; }
        public bool IsPermanent { get; }
        // public string Value { get; }

        public int Apply(int value);
    }
}
