using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class EscapeSupport : EscapeSupport<WarriorAI>
        {
            public override int Id => WarriorAIStateId.EscapeSupport;

            [Impl(256)] public EscapeSupport(WarriorAI parent) : base(parent) { }
        }
    }
}
