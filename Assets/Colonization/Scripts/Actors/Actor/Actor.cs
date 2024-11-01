using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    using FSMSelectable;
    using static CONST;

    public abstract partial class Actor : MonoBehaviour, ISelectable
    {
        protected int _id;
        protected Id<PlayerId> _owner;
        protected ActorSkin _skin;
        protected Transform _thisTransform;
        protected GameObject _thisGO;
        protected Hexagon _currentHex;
        protected readonly StateMachineSelectable _stateMachine = new();

        public Id<PlayerId> Owner => _owner;
        public Vector3 Position => _thisTransform.position;

        protected void Init(ActorSettings settings, int owner, ActorSkin skin, Hexagon startHex, GameplayEventBus eventBus)
        {
            _id = settings.Id;
            Debug.Log(_id);
            _owner = owner;
            _skin = skin;
            _currentHex = startHex;
            _thisTransform = transform;
            _thisGO = transform.gameObject;

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, ACTOR_ROTATIONS[_currentHex.GetNearGroundHexOffset()]);
            _currentHex.EnterActor(_owner);

            Skills skills = settings.Skills;

            _stateMachine.AddState(new IdleState(eventBus, this));
            _stateMachine.SetDefaultState<IdleState>();

            _stateMachine.AddState(skills.GetMoveSate(this));

            _stateMachine.Default();
            _thisGO.SetActive(true);
        }

        public bool CanMove()
        {
            return true;
        }
        public void Move()
        {
            _stateMachine.SetState<MoveState>();
        }


        public virtual void Select()
        {
            _stateMachine.Select();
        }

        public virtual void Unselect(ISelectable newSelectable)
        {
            _stateMachine.Unselect(newSelectable);
        }
    }
}
