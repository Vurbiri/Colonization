using System;

namespace Vurbiri.Colonization
{
    public class EdificeGroupId : AIdType<EdificeGroupId>
    {
        public const int None = 0;
        public const int Shrine = 1;
        public const int Port = 2;
        public const int Urban = 3;

        static EdificeGroupId() => RunConstructor();
        private EdificeGroupId() { }

        public static int ToIdAbility(int id) => id switch
        {
            None => PlayerStateId.None,
            Shrine => PlayerStateId.MaxShrine,
            Port => PlayerStateId.MaxPort,
            Urban => PlayerStateId.MaxUrban,
            _ => throw new ArgumentOutOfRangeException("self", $"����������� �������� EdificeGroup - {id} � ToAbilityType()"),
        };
    }
}
