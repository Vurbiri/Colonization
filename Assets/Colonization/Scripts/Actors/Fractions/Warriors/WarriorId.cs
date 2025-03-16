//Assets\Colonization\Scripts\Actors\Fractions\Warriors\WarriorId.cs
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract class WarriorId : ActorId<WarriorId>
    {
        public const int Militia    = 0;
        public const int Solder     = 1;
        public const int Wizard     = 2;
        public const int Saboteur   = 3;
        public const int Knight     = 4;

        static WarriorId() => RunConstructor();

        public static int ToState(int id) => id switch
        {
            Militia     => HumanAbilityId.IsMilitia,
            Solder      => HumanAbilityId.IsSolder,
            Wizard      => HumanAbilityId.IsWizard,
            Saboteur    => HumanAbilityId.IsSaboteur,
            Knight      => HumanAbilityId.IsKnight,
            _           => Errors.ArgumentOutOfRange("WarriorId", id),
        };
    }

    public static class ExtensionsWarriorId
    {
        public static int ToState(this Id<WarriorId> self) => WarriorId.ToState(self.Value);
    }
}
