using System;
using System.Runtime.CompilerServices;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.FSMSelectable
{
    public abstract class ASelectableState : IState, IEquatable<ASelectableState>
    {
        private readonly StateMachineSelectable _fsm;

        public ASelectableState(StateMachineSelectable fsm)
        {
            _fsm = fsm;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }

        public virtual void Cancel() { }
        public virtual void Select() { }
        public virtual void Unselect(ISelectable newSelectable) { }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void GetOutOfThisState() => _fsm.GetOutState(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void GetOutToPrevState() => _fsm.GetOutToPrevState(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ASelectableState other) => ReferenceEquals(this, other);
    }
}
