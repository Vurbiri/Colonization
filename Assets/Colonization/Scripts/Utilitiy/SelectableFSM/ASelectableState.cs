using Vurbiri.FSM;

namespace Vurbiri.Colonization.FSMSelectable
{
    public class ASelectableState : IState, ISelectable
    {
        protected readonly TypeIdKey _key;
        protected readonly StateMachineSelectable _fsm;

        public TypeIdKey Key => _key;

        public ASelectableState(StateMachineSelectable fsm, int id = 0)
        {
            _fsm = fsm;
            _key = new(GetType(), id);
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }

        public virtual void Select() { }
        public virtual void Unselect(ISelectable newSelectable) { }

        public virtual void Dispose() { }

        public override int GetHashCode() => _key.GetHashCode();
        public bool Equals(IState other) => other is not null && _key == other.Key;
        public override bool Equals(object obj) => Equals(obj as ASelectableState);

        public static bool operator ==(ASelectableState a, ASelectableState b) => (a is null && b is null) || (a is not null && b is not null && a._key == b._key);
        public static bool operator !=(ASelectableState a, ASelectableState b) => !(a == b);
        public static bool operator ==(ASelectableState s, TypeIdKey k) => s is not null && s._key == k;
        public static bool operator !=(ASelectableState s, TypeIdKey k) => s is not null && s._key != k;
        public static bool operator ==(TypeIdKey k, ASelectableState s) => s is not null && s._key == k;
        public static bool operator !=(TypeIdKey k, ASelectableState s) => s is not null && s._key != k;
    }
}
