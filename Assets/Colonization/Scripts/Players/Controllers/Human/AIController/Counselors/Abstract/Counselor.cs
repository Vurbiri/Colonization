using System.Collections;
using UnityEngine;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private abstract class Counselor
        {
            protected static readonly WaitRealtime s_waitRealtime = new(0.5f);

            protected readonly AIController _parent;

            #region Parent Properties
            protected AIController Human { [Impl(256)] get => _parent; }
            protected Id<PlayerId> HumanId { [Impl(256)] get => _parent._id; }
            protected Currencies Resources { [Impl(256)] get => _parent._resources; }
            protected int MaxResources { [Impl(256)] get => _parent._abilities[HumanAbilityId.MaxMainResources]; }
            protected ReadOnlyReactiveList<Crossroad> Ports { [Impl(256)] get => _parent._edifices.ports; }
            protected ReadOnlyReactiveList<Crossroad> Colonies { [Impl(256)] get => _parent._edifices.colonies; }
            protected Roads Roads { [Impl(256)] get => _parent._roads; }
            protected int FreeRoadCount { [Impl(256)] get => _parent._abilities[HumanAbilityId.MaxRoad] - _parent._roads.Count; }
            protected ReadOnlyAbilities<HumanAbilityId> Abilities { [Impl(256)] get => _parent._abilities; }
            protected PerkTree PerkTrees { [Impl(256)] get => _parent._perks; }
            protected bool IsEconomist { [Impl(256)] get => _parent._specialization == AbilityTypeId.Economic; }
            protected bool IsMilitarist { [Impl(256)] get => _parent._specialization == AbilityTypeId.Military; }
            #endregion

            [Impl(256)] public Counselor(AIController parent) => _parent = parent;

            public abstract IEnumerator Execution_Cn();

            [Impl(256)] protected Coroutine StartCoroutine(IEnumerator routine) => GameContainer.Shared.StartCoroutine(routine);
            [Impl(256)] protected void StopCoroutine(Coroutine coroutine) => GameContainer.Shared.StopCoroutine(coroutine);
        }
    }
}
