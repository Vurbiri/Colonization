//Assets\Colonization\Scripts\Actors\Skin\States\Abstract\ASkinState.cs
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour
    {
        public class ASkinState : IState
        {
            protected readonly TypeIdKey _key;
            protected readonly SkinStateMachine _fsm;
            protected readonly ActorSkin _parent;
            protected readonly Animator _animator;

            public TypeIdKey Key => _key;

            public ASkinState(ActorSkin parent, int id = 0)
            {
                _fsm = parent._stateMachine;
                _key = new(GetType(), id);
                _parent = parent;
                _animator = parent._animator;
            }

            public ASkinState()
            {
                _key = new(GetType(), 0);
            }

            public virtual void Enter() { }
            public virtual void Exit() { }
            public virtual void Update() { }

            public virtual void Dispose() { }

            public override int GetHashCode() => _key.GetHashCode();
            public bool Equals(IState other) => other is not null && _key == other.Key;
            public override bool Equals(object obj) => Equals(obj as AState);

            public static bool operator ==(ASkinState a, ASkinState b) => (a is null && b is null) || (a is not null && b is not null && a._key == b._key);
            public static bool operator !=(ASkinState a, ASkinState b) => !(a == b);
            public static bool operator ==(ASkinState a, IState b) => (a is null && b is null) || (a is not null && b is not null && a._key == b.Key);
            public static bool operator !=(ASkinState a, IState b) => !(a == b);
            public static bool operator ==(IState a, ASkinState b) => (a is null && b is null) || (a is not null && b is not null && a.Key == b._key);
            public static bool operator !=(IState a, ASkinState b) => !(a == b);
            public static bool operator ==(ASkinState s, TypeIdKey k) => s is not null && s._key == k;
            public static bool operator !=(ASkinState s, TypeIdKey k) => s is not null && s._key != k;
            public static bool operator ==(TypeIdKey k, ASkinState s) => s is not null && s._key == k;
            public static bool operator !=(TypeIdKey k, ASkinState s) => s is not null && s._key != k;
        }
    }
}
