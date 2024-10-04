using System;

namespace Vurbiri.Colonization
{
    public class IdEdificeGroup : AIdType<IdEdificeGroup>
    {
        public const int None = 0;
        public const int Shrine = 1;
        public const int Port = 2;
        public const int Urban = 3;

        static IdEdificeGroup() { RunConstructor(); }

        private IdEdificeGroup() { }

        public static int ToIdAbility(int id) => id switch
        {
            None => IdPlayerAbility.None,
            Shrine => IdPlayerAbility.MaxShrine,
            Port => IdPlayerAbility.MaxPort,
            Urban => IdPlayerAbility.MaxUrban,
            _ => throw new ArgumentOutOfRangeException("self", $"Неожидаемое значение EdificeGroup - {id} в ToAbilityType()"),
        };
    }
}
