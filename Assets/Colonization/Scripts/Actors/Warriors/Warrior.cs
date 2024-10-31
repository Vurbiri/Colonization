using UnityEngine;
using Vurbiri.Colonization.FSMSelectable;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    public class Warrior : MonoBehaviour, ISelectable
    {
        private float _speed = 0.5f;
        
        private int _id;
        private Transform _thisTransform;
        private GameObject _gameObject;
        private Hexagon _currentHex;
        private readonly StateMachineSelectable _stateMachine = new();

        public Id<PlayerId> Owner { get; private set; }
        public ActorSkin Skin { get; private set; }
        public Hexagon CurrentHex => _currentHex;
        public Transform Transform => _thisTransform;
        public Vector3 Position => _thisTransform.position;
        public StateMachineSelectable FSM => _stateMachine;

        public Warrior Init(int id, int owner, ActorSkin skin, Hexagon startHex, GameplayEventBus eventBus)
        {
            _id = id;
            Owner = owner;
            Skin = skin;
            _currentHex = startHex;
            _thisTransform = transform;
            _gameObject = transform.gameObject;

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, ACTOR_ROTATIONS[_currentHex.GetNearGroundHexOffset()]);
            _currentHex.EnterActor(owner);

            _stateMachine.AddState(new IdleState(eventBus, this));
            _stateMachine.SetDefaultState<IdleState>();
            _stateMachine.AddState(new MoveState(_speed, SetCurrentHex, this));

            _stateMachine.Reset();
            _gameObject.SetActive(true);
            
            return this;
        }

        public bool CanMove()
        {
            return true;
        }

        public void Move()
        {
            _stateMachine.SetState<MoveState>();
        }

        public void Select()
        {
            _stateMachine.Select();
        }

        public void Unselect(ISelectable newSelectable)
        {
            _stateMachine.Unselect(newSelectable);
        }

        private void SetCurrentHex(Hexagon hex) => _currentHex = hex;
    }
}
