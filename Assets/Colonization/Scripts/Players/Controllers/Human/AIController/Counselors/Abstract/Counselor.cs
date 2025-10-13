using System.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private abstract class Counselor
        {
            protected readonly AIController _parent;

            protected AIController Human { [Impl(256)] get => _parent; }
            protected int Id { [Impl(256)] get => _parent._id; }
            protected Currencies Resources { [Impl(256)] get => _parent._resources; }
            protected int MaxResources { [Impl(256)] get => _parent._abilities[HumanAbilityId.MaxMainResources]; }
            protected ReadOnlyReactiveList<Crossroad> Ports { [Impl(256)] get => _parent._edifices.ports; }
            protected ReadOnlyReactiveList<Crossroad> Colonies { [Impl(256)] get => _parent._edifices.colonies; }
            protected Roads Roads { [Impl(256)] get => _parent._roads; }
            protected int FreeRoadCount { [Impl(256)] get => _parent._abilities[HumanAbilityId.MaxRoad] - _parent._roads.Count; }
            protected ReadOnlyAbilities<HumanAbilityId> Abilities { [Impl(256)] get => _parent._abilities; }

            [Impl(256)] public Counselor(AIController parent) => _parent = parent;

            public abstract IEnumerator Init_Cn();
            public abstract IEnumerator Planning_Cn();
            public abstract IEnumerator Execution_Cn();
        }
    }
}
