namespace Vurbiri.FSM
{
    public abstract class AState : IState
    {
        protected readonly TypeIdKey _key;
        protected readonly StateMachine _fsm;

        public TypeIdKey Key => _key;

        public AState(StateMachine fsm, int id = 0)
        {
            _fsm = fsm;
            _key = new(GetType(), id);
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }

        public virtual void Dispose() { }

        public override int GetHashCode() => _key.GetHashCode();
        public bool Equals(IState other) => other is not null && _key == other.Key;
        public override bool Equals(object obj) => Equals(obj as AState);

        public static bool operator ==(AState a, AState b) => (a is null && b is null) || (a is not null && b is not null && a._key == b._key);
        public static bool operator !=(AState a, AState b) =>!(a == b);
        public static bool operator ==(AState a, IState b) => (a is null && b is null) || (a is not null && b is not null && a._key == b.Key);
        public static bool operator !=(AState a, IState b) => !(a == b);
        public static bool operator ==(IState a, AState b) => (a is null && b is null) || (a is not null && b is not null && a.Key == b._key);
        public static bool operator !=(IState a, AState b) => !(a == b);
        public static bool operator ==(AState s, TypeIdKey k) => s is not null && s._key == k;
        public static bool operator !=(AState s, TypeIdKey k) => s is not null && s._key != k;
        public static bool operator ==(TypeIdKey k, AState s) => s is not null && s._key == k;
        public static bool operator !=(TypeIdKey k, AState s) => s is not null && s._key != k;
    }
}
