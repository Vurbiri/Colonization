namespace Vurbiri.Colonization
{
    public interface IPerk
    {
        public int Id { get; }
        public int Level { get; }
        public TargetObjectPerk TargetObject { get; }
        public PlayerAbilityType TargetAbility { get; }
        public Currencies Cost { get; }
        public bool IsPermanent { get; }
        // public string Value { get; }

        public int Apply(int value);
    }
}
