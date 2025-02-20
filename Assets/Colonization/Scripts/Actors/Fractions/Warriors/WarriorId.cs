//Assets\Colonization\Scripts\Actors\Fractions\Warriors\WarriorId.cs
using System;
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
            Militia     => PlayerAbilityId.IsMilitia,
            Solder      => PlayerAbilityId.IsSolder,
            Wizard      => PlayerAbilityId.IsWizard,
            Saboteur    => PlayerAbilityId.IsSaboteur,
            Knight      => PlayerAbilityId.IsKnight,
            _           => throw new ArgumentOutOfRangeException("id", $"WarriorId: {id}.ToState()"),
        };
    }

    public static class ExtensionsWarriorId
    {
        public static int ToState(this Id<WarriorId> self) => WarriorId.ToState(self.Value);
    }
}
