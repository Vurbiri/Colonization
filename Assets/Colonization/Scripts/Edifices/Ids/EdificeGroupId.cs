namespace Vurbiri.Colonization
{
    using Characteristics;
    using System.Runtime.CompilerServices;

    public class EdificeGroupId : IdType<EdificeGroupId>
    {
        public const int Shrine  =  0;
        public const int Colony  =  1;
        public const int Port    =  2;
        

        static EdificeGroupId() => ConstructorRun();
        private EdificeGroupId() { }

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
