using System;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.FSMSelectable;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract partial class Actor : AReactiveItemMono<Actor>, IInteractable, IDisposable
    {
        public enum DeathStage
        {
            None, Start, Animation, SFX
        }

        #region Fields
        protected Id<ActorTypeId> _typeId;
        protected int _id;
        protected Id<PlayerId> _owner;
        private bool _isPersonTurn, _canUseSkills;
        #region Abilities
        protected AbilitiesSet<ActorAbilityId> _abilities;
        protected SubAbility<ActorAbilityId> _currentHP;
        protected SubAbility<ActorAbilityId> _currentAP;
        protected BooleanAbility<ActorAbilityId> _move;
        protected ChanceAbility<ActorAbilityId> _profitMain;
        protected ChanceAbility<ActorAbilityId> _profitAdv;
        #endregion

        protected Hexagon _currentHex;

        protected ActorSkin _skin;
        protected Transform _thisTransform;
        protected BoxCollider _thisCollider;

        protected float _extentsZ;

        protected EffectsSet _effects;

        #region States
        protected readonly StateMachineSelectable _stateMachine = new();
        private readonly TargetState _targetState = new();
        private MoveState _moveState;
        private ASkillState[] _skillState;
        private DeathState _deathState;
        #endregion

        private readonly RBool _interactable = new(false);
        private readonly RBool _canCancel = new(false);

        private Unsubscriptions _unsubscribers = new();
        #endregion

        #region Propirties
        public Id<ActorTypeId> TypeId => _typeId;
        public int Id => _id;
        public Id<PlayerId> Owner => _owner;
        public Hexagon Hexagon => _currentHex;
        public int ActionPoint => _currentAP.Value;
        public bool CanMove => _move.IsValue;
        public bool CanUseSkills => _canUseSkills & _isPersonTurn;
        public int CurrentHP => _currentHP.Value;
        public bool IsWounded => _currentHP.IsNotMax;
        public bool IsDead => _currentHP.Value <= 0;
        public ActorSkin Skin => _skin;
        public IReactiveSet<ReactiveEffect> Effects => _effects;
        public AbilitiesSet<ActorAbilityId> Abilities => _abilities;
        public bool IsMainProfit => _profitMain.Next();
        public bool IsAdvProfit => _profitAdv.Next();
        #endregion

        #region IInteractable
        public Vector3 Position => _thisTransform.position;
        public RBool CanCancel => _canCancel;
        public RBool InteractableReactive => _interactable;
        public bool Interactable { get => _interactable.Value; set => _thisCollider.enabled = _interactable.Value = _isPersonTurn & value; }
        public bool RaycastTarget { get => _thisCollider.enabled; set => _thisCollider.enabled = _isPersonTurn | value; }
        public bool IsPersonTurn { get => _isPersonTurn; set => _isPersonTurn = value; }
        public void Select() => _stateMachine.Select();
        public void Unselect(ISelectable newSelectable) => _stateMachine.Unselect(newSelectable);
        public void Cancel() => _stateMachine.Cancel();
        #endregion

        public abstract bool IsAvailableStateMachine { get; }

        #region States
        public abstract WaitSignal UseSpecSkill();

        public WaitSignal Move()
        {
            _stateMachine.SetState(_moveState, true);
            return _moveState.Signal;
        }
        public WaitSignal UseSkill(int id)
        {
            _stateMachine.SetState(_skillState[id], true);
            return _skillState[id].Signal;
        }
        #endregion

        public bool IsCanApplySkill(Id<PlayerId> id, Relation typeAction, out bool isFriendly)
        {
            return IsAvailableStateMachine & GameContainer.Diplomacy.IsCanActorsInteraction(id, _owner, typeAction, out isFriendly);
        }

        #region Effect
        public int AddEffect(ReactiveEffect effect) => _effects.Add(effect);
        public int ApplyEffect(IPerk effect)
        {
            int delta = _abilities.AddPerk(effect);

            if(delta != 0 & _deathState == null)
                _eventChanged.Invoke(this, TypeEvent.Change);

            return delta;
        }
        public void ClearEffects(int duration, Id<ClearEffectsId> type) => _effects.Degrade(duration, type);
        #endregion

        #region Start/End turn
        public void StatesUpdate()
        {
            _currentHP.Next();
            _currentAP.Next();
            _move.On();
        }
        public void EffectsUpdate(int defense)
        {
            _effects.Next();
            _effects.Add(ReactiveEffectsFactory.CreateWallDefenceEffect(defense));

            _stateMachine.ToDefaultState();
        }
        public void EffectsUpdate() => EffectsUpdate(_currentHex.GetMaxDefense());
        #endregion

        #region WallDefence
        public void AddWallDefenceEffect(int maxDefense) => _effects.Add(ReactiveEffectsFactory.CreateWallDefenceEffect(maxDefense));
        public void RemoveWallDefenceEffect() => _effects.Remove(ReactiveEffectsFactory.WallEffectCode);
        #endregion

        public void SetHexagonSelectable()
        {
            _currentHex.SetSelectableForSwap();
            Interactable = false;
        }
        public void SetHexagonUnselectable()
        {
            _currentHex.SetUnselectableForSwap();
            Interactable = IsAvailableStateMachine;
        }

        public WaitStateSource<DeathStage> Death()
        {
            _stateMachine.ForceSetState(_deathState = new(this), true);

            return _deathState.stage;
        }

        public bool Equals(ISelectable other) => System.Object.ReferenceEquals(this, other);
        sealed public override bool Equals(Actor other) => System.Object.ReferenceEquals(this, other);
        sealed public override void Removing()
        {
            _currentHex.ExitActor();
            _unsubscribers.Unsubscribe();

            _eventChanged.Invoke(this, TypeEvent.Remove);
            _eventChanged.Clear();
            _index = -1;

            _effects.Dispose();
        }

        sealed public override void Dispose() { }

        #region Target
        private bool ToTargetState(Id<PlayerId> initiator, Relation relation)
        {
            bool isSet = GameContainer.Diplomacy.IsCanActorsInteraction(initiator, _owner, relation, out _) && _stateMachine.SetState(_targetState, true);
            if(isSet)
                GameContainer.Diplomacy.ActorsInteraction(_owner, initiator, relation);
            return isSet;
        }

        private void FromTargetState()
        {
            if (_deathState == null)
            {
                _stateMachine.GetOutToPrevState(_targetState);
                _eventChanged.Invoke(this, TypeEvent.Change);
            }
        }
        #endregion

        private void OnBuff(IPerk perk)
        {
            if (_abilities.AddPerk(perk) != 0)
                _eventChanged.Invoke(this, TypeEvent.Change);
        }

        private void RedirectEvents(ReactiveEffect item, TypeEvent type) => _eventChanged.Invoke(this, TypeEvent.Change);
        private void Signal() => _eventChanged.Invoke(this, TypeEvent.Change);
    }
}
