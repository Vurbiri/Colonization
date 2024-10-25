using System;

namespace Vurbiri.Colonization
{
    public class EdificeId : AIdType<EdificeId>
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

        static EdificeId() => RunConstructor();
        private EdificeId() { }

        public static int ToGroup(int id) => id switch
        {
            None                                                 => EdificeGroupId.None,
            Shrine                                               => EdificeGroupId.Shrine,
            PortOne or PortTwo or LighthouseOne or LighthouseTwo => EdificeGroupId.Port,
            Camp or Town or Capital                              => EdificeGroupId.Urban,
            _                                                    => throw new ArgumentOutOfRangeException("self", $"Неожидаемое значение EdificeType: {id}"),
        };

        public static int GetId(int countWater, bool isGate) => countWater switch
        {
            0 when isGate  => Shrine,
            0 when !isGate => Camp,
            1              => PortOne,
            2              => PortTwo,
            _              => None
        };
    }

    public static class ExtensionsEdificeId
    {
        public static int ToGroup(this Id<EdificeId> self) => EdificeId.ToGroup(self.Value); 
    }
}
