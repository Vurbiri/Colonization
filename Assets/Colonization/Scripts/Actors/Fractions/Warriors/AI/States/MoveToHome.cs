using static Vurbiri.Colonization.Actor;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToHome : MoveToColony
        {
            [Impl(256)] public MoveToHome(AI<WarriorsAISettings, WarriorId, WarriorAIStateId> parent) : base(parent, 1) => _owners.Add(OwnerId);

            public override bool TryEnter()
            {
                _targetHexagon = null;
                return (Status.isMove & !(Status.isSiege | Status.isGuard)) && TrySetColony(s_settings.maxDistanceHome, 2);
            }
        }
    }
}
