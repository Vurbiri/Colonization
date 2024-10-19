using System;

namespace Vurbiri.FSM
{
    public abstract class AState : IDisposable
    {
        protected StateMachine _fsm;

        public AState(StateMachine fsm) => _fsm = fsm;

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void Update() { }


        public virtual void Dispose() { }

        public override string ToString() => GetType().Name;

        public override int GetHashCode() => GetType().GetHashCode();
        public bool Equals(AState other) => other is not null && GetType() == other.GetType();
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is Type type) return GetType() == type;
            return obj is AState state && GetType() == state.GetType();
        }
        public static bool operator ==(AState a, AState b) => (a is null && b is null) || (a is not null && b is not null && a.GetType() == b.GetType());
        public static bool operator !=(AState a, AState b) =>!(a == b);
        public static bool operator ==(AState s, Type t) => s is not null && t is not null && s.GetType() == t;
        public static bool operator !=(AState s, Type t) => !(s == t);
        public static bool operator ==(Type t, AState s) => s is not null && t is not null && s.GetType() == t;
        public static bool operator !=(Type t, AState s) => !(s == t);
    }
}
