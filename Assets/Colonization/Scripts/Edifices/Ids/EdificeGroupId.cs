using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization
{
    public abstract class EdificeGroupId : IdType<EdificeGroupId>
    {
        public const int Shrine  =  0;
        public const int Colony  =  1;
        public const int Port    =  2;

        public static int ToState(int id) => id switch
        {
            Shrine   => HumanAbilityId.MaxShrine,
            Port     => HumanAbilityId.MaxPort,
            Colony   => HumanAbilityId.MaxColony,
            _        => Errors.ArgumentOutOfRange("EdificeGroupId", id),
        };
    }

    public static class ExtensionsEdificeGroupId
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToState(this Id<EdificeGroupId> self) => EdificeGroupId.ToState(self.Value);
    }
}
