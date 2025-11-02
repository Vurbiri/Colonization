using System.Collections;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        private abstract class AIState
        {
            private readonly WarriorAI _parent;

            protected Id<PlayerId> _playerId;

            #region Parent Properties
            protected Actor Actor { [Impl(256)] get => _parent._actor; }
            protected Status Status { [Impl(256)] get => _parent._status; }
            protected bool ActorInCombat { [Impl(256)] get => _parent._actor.IsInCombat(); }
            protected Warrior.WarriorStates Action { [Impl(256)] get => _parent._action; }
            protected ReadOnlyReactiveList<Crossroad> Colonies { [Impl(256)] get => GameContainer.Players.Humans[_playerId].Colonies; }
            #endregion

            [Impl(256)] protected AIState(WarriorAI parent)
            {
                _parent = parent;
                _playerId = parent._actor.Owner;
            }

            public abstract bool TryEnter();
            protected abstract void OnExit();
            public abstract IEnumerator Execution_Cn(Out<bool> isContinue);

            public override string ToString() => GetType().Name;

            [Impl(256)] protected bool TryEnter(AIState state)
            {
                bool result = state.TryEnter();
                if (result)
                    _parent._current = state;
                return result;
            }

            [Impl(256)] protected void Exit()
            {
                OnExit();
                _parent._current = _parent._goalSetting;
            }
        }
    }
}
