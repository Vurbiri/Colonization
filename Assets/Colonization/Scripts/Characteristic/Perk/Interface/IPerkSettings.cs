namespace Vurbiri.Colonization
{
    public interface IPerkSettings : IPerk
    {
        public int Id { get; }
        public int Type { get; }
        public int Level { get; }
        public Id<TargetOfPerkId> TargetObject { get; }
        public int PrevPerk { get; }
        public CurrenciesLite Cost { get; }
    }
}
