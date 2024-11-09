using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    using FSMSelectable;
    using System.Collections.Generic;
    using static CONST;

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

        public virtual void Init(ActorSettings settings, int owner, Hexagon startHex, GameplayEventBus eventBus)
        {
            _id = settings.Id;
            _owner = owner;
            _states = settings.Abilities;
            _skin = settings.InstantiateActorSkin(transform);
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

            List<AttackState> attackStates = skills.GetAttackSates(this);
            _countAttackStates = attackStates.Count;
            for(int i = 0; i < _countAttackStates; i++)
                _stateMachine.AddState(attackStates[i]);

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
