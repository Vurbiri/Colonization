//Assets\Colonization\Scripts\Edifices\Ids\EdificeId.cs
namespace Vurbiri.Colonization
{
    public abstract class EdificeId : IdType<EdificeId>
    {
        public const int Empty          = 0;

        public const int Shrine         = 1;

        public const int PortOne        = 2;
        public const int PortTwo        = 3;
        public const int LighthouseOne  = 4;
        public const int LighthouseTwo  = 5;

        public const int Camp           = 6;
        public const int Town           = 7;
        public const int Capital        = 8;

        static EdificeId() => RunConstructor();

        public static int ToGroup(int id) => id switch
        {
            Empty                                                => EdificeGroupId.None,
            Shrine                                               => EdificeGroupId.Shrine,
            PortOne or PortTwo or LighthouseOne or LighthouseTwo => EdificeGroupId.Port,
            Camp or Town or Capital                              => EdificeGroupId.Urban,
            _                                                    => Errors.ArgumentOutOfRange("EdificeTypeId", id),
        };

        public static int GetId(int countWater, bool isGate) => countWater switch
        {
            0 when isGate  => Shrine,
            0 when !isGate => Camp,
            1              => PortOne,
            2              => PortTwo,
            _              => Empty
        };
    }

    public static class ExtensionsEdificeId
    {
        public static Id<EdificeGroupId> ToGroup(this Id<EdificeId> self) => EdificeId.ToGroup(self.Value); 
    }
}
