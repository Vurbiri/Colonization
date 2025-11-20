using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class Support : Support<WarriorAI>
        {
            public override int Id => WarriorAIStateId.Support;

            [Impl(256)]
            public Support(WarriorAI parent) : base(parent) { }
        }
    }
}
