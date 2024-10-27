using System;

namespace Vurbiri.Colonization
{
    public class WarriorId : AIdType<WarriorId>
    {
        public const int Militia    = 0;
        public const int Solder     = 1;
        public const int Wizard     = 2;
        public const int Saboteur   = 3;
        public const int Knight     = 4;

        static WarriorId() => RunConstructor();
        private WarriorId() { }

        public static int ToState(int id) => id switch
        {
            Militia     => PlayerStateId.IsMilitia,
            Solder      => PlayerStateId.IsSolder,
            Wizard      => PlayerStateId.IsWizard,
            Saboteur    => PlayerStateId.IsSaboteur,
            Knight      => PlayerStateId.IsKnight,
            _           => throw new ArgumentOutOfRangeException("id", $"Неожидаемое значение WarriorId - {id} в ToState()"),
        };
    }

    public static class ExtensionsWarriorId
    {
        public static int ToState(this Id<WarriorId> self) => WarriorId.ToState(self.Value);
    }
}
