using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    using FSMSelectable;
    using static CONST;

    [RequireComponent(typeof(BoxCollider))]
    public abstract partial class Actor : MonoBehaviour, ISelectable
    {
        protected int _id;
        protected Id<PlayerId> _owner;
        protected StatesSet<ActorStateId> _states;
        protected ActorSkin _skin;
        protected Transform _thisTransform;
        protected GameObject _thisGO;
        protected Hexagon _currentHex;
        protected GameplayEventBus _eventBus;
        protected float _extentsZ;
        protected readonly StateMachineSelectable _stateMachine = new();
       
        public Id<PlayerId> Owner => _owner;
        public Vector3 Position => _thisTransform.position;

        protected void Init(ActorSettings settings, int owner, ActorSkin skin, Hexagon startHex, GameplayEventBus eventBus)
        {
            _id = settings.Id;
            _owner = owner;
            _states = settings.States;
            _skin = skin;
            _currentHex = startHex;
            _eventBus = eventBus;
            _thisTransform = transform;
            _thisGO = transform.gameObject;
            _extentsZ = GetComponent<BoxCollider>().bounds.extents.z;

            _skin.EventStart += _stateMachine.Default;

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, ACTOR_ROTATIONS[_currentHex.GetNearGroundHexOffset()]);
            _currentHex.EnterActor(this);

            Skills skills = settings.Skills;

            _stateMachine.AddState(new IdleState(this));
            _stateMachine.SetDefaultState<IdleState>();

            _stateMachine.AddState(skills.GetMoveSate(this));
            _stateMachine.AddState(new AttackState(this));

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

        public void Attack()
        {
            _stateMachine.SetState<AttackState>();
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
