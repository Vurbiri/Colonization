//Assets\Colonization\Scripts\Edifices\Ids\EdificeGroupId.cs
namespace Vurbiri.Colonization
{
    using Characteristics;
    using System;

    public class EdificeGroupId : IdType<EdificeGroupId>
    {
        public const int None   = -1;
        public const int Shrine =  0;
        public const int Port   =  1;
        public const int Urban  =  2;

        static EdificeGroupId() => RunConstructor();
        private EdificeGroupId() { }

        public static int ToState(int id) => id switch
        {
            Shrine  => PlayerAbilityId.MaxShrine,
            Port    => PlayerAbilityId.MaxPort,
            Urban   => PlayerAbilityId.MaxUrban,
            _       => throw new ArgumentOutOfRangeException("id", $"EdificeGroup: {id}.  ToState(..)"),
        };
    }

    public static class ExtensionsEdificeGroupId
    {
        public static int ToState(this Id<EdificeGroupId> self) => EdificeGroupId.ToState(self.Value);
    }
}
