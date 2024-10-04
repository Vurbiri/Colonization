using System;

namespace Vurbiri.Colonization
{
    public class IdEdifice : AIdType<IdEdifice>
    {
        public const int None = 0;

        public const int Shrine = 1;

        public const int PortOne = 2;
        public const int PortTwo = 3;
        public const int LighthouseOne = 4;
        public const int LighthouseTwo = 5;

        public const int Camp = 6;
        public const int Town = 7;
        public const int Capital = 8;

        static IdEdifice() { RunConstructor(); }

        private IdEdifice() { }

        public static int ToGroup(int id) => id switch
        {
            None => IdEdificeGroup.None,
            Shrine => IdEdificeGroup.Shrine,
            PortOne or PortTwo or LighthouseOne or LighthouseTwo => IdEdificeGroup.Port,
            Camp or Town or Capital => IdEdificeGroup.Urban,
            _ => throw new ArgumentOutOfRangeException("self", $"Неожидаемое значение EdificeType: {id}"),
        };
    }
}
