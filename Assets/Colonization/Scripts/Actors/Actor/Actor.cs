namespace Vurbiri.Colonization.Actors
{
    using FSMSelectable;
    using System;
    using UnityEngine;
    using Vurbiri.Reactive.Collections;

    [RequireComponent(typeof(BoxCollider))]
    public abstract partial class Actor : MonoBehaviour, ISelectable, IReactiveElement<Actor>
    {
        #region Fields
        protected int _id;
        protected Id<PlayerId> _owner;
        protected int _currentHP;
        protected int _currentActionPoint = 3;
        protected Hexagon _currentHex;

        protected int _index = -1;
        protected AbilitiesSet<ActorAbilityId> _states;
        protected ActorSkin _skin;
        protected Transform _thisTransform;
        protected GameObject _thisGO;
        protected GameplayEventBus _eventBus;
        protected float _extentsZ;

        protected readonly ReactiveCollection<Effect> _effects = new();
        protected readonly StateMachineSelectable _stateMachine = new();
        protected int _countAttackStates;
        protected Action<Actor, Operation> actionThisChange;
        #endregion

        public int Id => _id;
        public int Index { get => _index; set => _index = value; }
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

        public void Subscribe(Action<Actor, Operation> action, int index)
        {
            actionThisChange += action ?? throw new ArgumentNullException("action");
            _index = index;
            action(this, Operation.Add);
        }

        public int[][] ToArray()
        {
            int count = _effects.Count + 2;
            int[][] array = new int[count][];
            array[0] = _currentHex.Key.ToArray();
            array[1] = new int[] { _id, _currentHP, _currentActionPoint };
            for (int i = 2; i < count; i++)
                array[i] = _effects[i].ToArray();

            return array;
        }

        private void AddEffect(EffectSettings effectSettings) => _effects.Add(effectSettings);

        protected virtual void Removing()
        {
            actionThisChange?.Invoke(this, Operation.Remove);
            actionThisChange = null;
            Destroy(_thisGO);
        }

        private void RedirectEvents(Effect item, Operation operation)
        {
            if (operation == Operation.Add)
                _states.AddPerk(item);
            else if (operation == Operation.Remove)
                _states.RemovePerk(item);

            actionThisChange?.Invoke(this, Operation.Change);
        }
    }
}
