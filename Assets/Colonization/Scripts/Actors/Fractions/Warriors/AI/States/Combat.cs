using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class Combat : Combat<WarriorAI>
        {
            public override int Id => WarriorAIStateId.Combat;

            [Impl(256)] public Combat(WarriorAI parent) : base(parent) { }
        }
    }
}
