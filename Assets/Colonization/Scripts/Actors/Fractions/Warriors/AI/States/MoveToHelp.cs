using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToHelp : MoveToHelp<WarriorAI>
        {
            public override int Id => WarriorAIStateId.MoveToHelp;

            [Impl(256)] 
            public MoveToHelp(WarriorAI parent) : base(parent) { }
        }
    }
}
