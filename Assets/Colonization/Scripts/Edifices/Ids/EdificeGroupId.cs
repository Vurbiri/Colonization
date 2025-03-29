//Assets\Colonization\Scripts\Edifices\Ids\EdificeGroupId.cs
namespace Vurbiri.Colonization
{
    using Characteristics;

    public class EdificeGroupId : IdType<EdificeGroupId>
    {
        public const int None   = -1;
        public const int Shrine =  0;
        public const int Urban  =  1;
        public const int Port   =  2;
        

        static EdificeGroupId() => RunConstructor();
        private EdificeGroupId() { }

        public static int ToState(int id) => id switch
        {
            Shrine  => HumanAbilityId.MaxShrine,
            Port    => HumanAbilityId.MaxPort,
            Urban   => HumanAbilityId.MaxUrban,
            _       => Errors.ArgumentOutOfRange("EdificeGroupId", id),
        };
    }

    public static class ExtensionsEdificeGroupId
    {
        public static int ToState(this Id<EdificeGroupId> self) => EdificeGroupId.ToState(self.Value);
    }
}
