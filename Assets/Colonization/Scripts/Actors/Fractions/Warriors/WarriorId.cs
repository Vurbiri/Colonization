using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract class WarriorId : ActorId<WarriorId>
    {
        public const int Militia    = 0;
        public const int Solder     = 1;
        public const int Wizard     = 2;
        public const int Warlock    = 3;
        public const int Knight     = 4;

        static WarriorId() => ConstructorRun();

        public static int ToState(int id) => id switch
        {
            Militia     => HumanAbilityId.IsMilitia,
            Solder      => HumanAbilityId.IsSolder,
            Wizard      => HumanAbilityId.IsWizard,
            Warlock     => HumanAbilityId.IsWarlock,
            Knight      => HumanAbilityId.IsKnight,
            _           => Errors.ArgumentOutOfRange("WarriorId", id),
        };
    }

    public static class ExtensionsWarriorId
    {
        public static int ToState(this Id<WarriorId> self) => WarriorId.ToState(self.Value);
    }
}
