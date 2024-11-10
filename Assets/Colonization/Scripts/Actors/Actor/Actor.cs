namespace Vurbiri.Colonization.Actors
{
    using FSMSelectable;
    using UnityEngine;

    [RequireComponent(typeof(BoxCollider))]
    public abstract partial class Actor : MonoBehaviour, ISelectable
    {
        protected int _id;
        protected Id<PlayerId> _owner;
        protected int _currentHP;
        protected int _currentActionPoint = 3;

        protected AbilitiesSet<ActorAbilityId> _states;
        protected ActorSkin _skin;
        protected Transform _thisTransform;
        protected GameObject _thisGO;
        protected Hexagon _currentHex;
        protected GameplayEventBus _eventBus;
        protected float _extentsZ;

        protected readonly StateMachineSelectable _stateMachine = new();
        protected int _countAttackStates;
       
        public int Id => _id;
        public Id<PlayerId> Owner => _owner;
        public int ActionPoint => _currentActionPoint;
        public Vector3 Position => _thisTransform.position;
        public bool CanAction => true;


        public bool CanMove()
        {
            return true;
        }
        public void Move()
        {
            _stateMachine.SetState<MoveState>();
        }

        public void Attack(int id)
        {
            _stateMachine.SetState<AttackState>(id);
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
