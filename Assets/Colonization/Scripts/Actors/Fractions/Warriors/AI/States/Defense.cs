using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class Defense : Defense<WarriorAI>
        {
             public override int Id => WarriorAIStateId.Defense;

            [Impl(256)] public Defense(WarriorAI parent) : base(parent) { }
        }
    }
}
