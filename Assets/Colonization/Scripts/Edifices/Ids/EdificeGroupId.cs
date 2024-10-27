using System;

namespace Vurbiri.Colonization
{
    public class EdificeGroupId : AIdType<EdificeGroupId>
    {
        public const int None   = -1;
        public const int Shrine = 0;
        public const int Port   = 1;
        public const int Urban  = 2;

        static EdificeGroupId() => RunConstructor();
        private EdificeGroupId() { }

        public static int ToState(int id) => id switch
        {
            None    => PlayerStateId.None,
            Shrine  => PlayerStateId.MaxShrine,
            Port    => PlayerStateId.MaxPort,
            Urban   => PlayerStateId.MaxUrban,
            _       => throw new ArgumentOutOfRangeException("id", $"Неожидаемое значение EdificeGroup - {id} в ToState()"),
        };
    }

    public static class ExtensionsEdificeGroupId
    {
        public static int ToState(this Id<EdificeGroupId> self) => EdificeGroupId.ToState(self.Value);
    }
}
