//Assets\Colonization\Scripts\Actors\Actor\Actor.cs
using System;
using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.FSMSelectable;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract partial class Actor : AReactiveItemMono<Actor>, ISelectable, IDisposable, IPositionable, ICancel
    {
        #region Fields
        private int _typeId;
        private int _id;
        private Id<PlayerId> _owner;
        private bool _isPlayerTurn;

        #region Abilities
        private AbilitiesSet<ActorAbilityId> _abilities;
        private SubAbility<ActorAbilityId> _currentHP;
        private SubAbility<ActorAbilityId> _currentAP;
        private BooleanAbility<ActorAbilityId> _move;
        private ChanceAbility<ActorAbilityId> _profitMain;
        private ChanceAbility<ActorAbilityId> _profitAdv;
        #endregion

        private Hexagon _currentHex;

        private ActorSkin _skin;
        private Transform _thisTransform;
        private Collider _thisCollider;
        private Diplomacy _diplomacy;
        private GameplayTriggerBus _triggerBus;
        private float _extentsZ;

        private EffectsSet _effects;

        private StateMachineSelectable _stateMachine;
        private BlockState _blockState;

        private readonly RBool _interactable = new(false);
        private readonly RBool _canCancel = new(false);

        private Coroutine _deathCoroutine;
        private Unsubscribers _unsubscribers = new();
        #endregion

        #region Propirties
        public int TypeId => _typeId;
        public int Id => _id;
        public Id<PlayerId> Owner => _owner;
        public Hexagon Hexagon => _currentHex;
        public int ActionPoint => _currentAP.Value;
        public bool CanMove => _move.IsValue;
        public bool IsIdle => _stateMachine.IsDefaultState;
        public bool IsBlock => _blockState.Enabled;
        public bool IsDead => _currentHP.Value <= 0;
        public Vector3 Position => _thisTransform.position;
        public ActorSkin Skin => _skin;
        public IReactiveSet<ReactiveEffect> Effects => _effects;
        public AbilitiesSet<ActorAbilityId> Abilities => _abilities;
        public bool IsMainProfit => _profitMain.Next();
        public bool IsAdvProfit => _profitAdv.Next();

        #endregion

        #region ISelectable, ICancel
        public RBool CanCancel => _canCancel;
        public RBool InteractableReactive => _interactable;
        public bool Interactable { get => _interactable.Value; private set => _thisCollider.enabled = _interactable.Value = _isPlayerTurn & value; }
        public bool RaycastTarget { get => _thisCollider.enabled; set => _thisCollider.enabled = value; }
        public bool IsPlayerTurn { get => _isPlayerTurn; set => _interactable.Value = _isPlayerTurn = value; }
        public void Select() => _stateMachine.Select();
        public void Unselect(ISelectable newSelectable) => _stateMachine.Unselect(newSelectable);
        public void Cancel() => _stateMachine.Cancel();
        #endregion

        #region States
        public void Move() => _stateMachine.SetState<MoveState>();
        public void Block() => _stateMachine.SetState(_blockState);
        public void UseSkill(int id) => _stateMachine.SetState<ASkillState>(id);
        #endregion

        public Relation GetRelation(Id<PlayerId> id) => _diplomacy.GetRelation(id, _owner);
        public bool IsCanUseSkill(Id<PlayerId> id, Relation typeAction, out bool isFriendly)
        {
            if(!(_stateMachine.IsDefaultState || _blockState.Enabled))
            {
                isFriendly = false;
                return false;
            }
            
            return _diplomacy.IsCanActorsInteraction(id, _owner, typeAction, out isFriendly);
        }

        #region Effect
        public int AddEffect(ReactiveEffect effect) => _effects.Add(effect);
        public int ApplyEffect(IPerk effect)
        {
            int delta = _abilities.AddPerk(effect);

            if(delta != 0 & _deathCoroutine == null)
                _signer.Invoke(this, TypeEvent.Change);

            return delta;
        }
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
            _effects.Add(EffectsFactory.CreateWallDefenceEffect(defense));

            _stateMachine.ToDefaultState();
        }
        public void EffectsUpdate() => EffectsUpdate(_currentHex.GetMaxDefense());
        #endregion

        #region WallDefence
        public void AddWallDefenceEffect(int maxDefense) => _effects.Add(EffectsFactory.CreateWallDefenceEffect(maxDefense));
        public void RemoveWallDefenceEffect() => _effects.Remove(EffectsFactory.WallEffectCode);
        #endregion

        public bool Equals(ISelectable other) => System.Object.ReferenceEquals(this, other);
        sealed public override bool Equals(Actor other) => System.Object.ReferenceEquals(this, other);
        sealed public override void Dispose()
        {
            _currentHex.ExitActor();

            _skin.Dispose();
            _stateMachine.Dispose();
            _effects.Dispose();

            Destroy(gameObject);
        }

        #region Target
        private void ToTargetState(Id<PlayerId> initiator, Relation relation)
        {
            _stateMachine.SetState<BecomeTargetState>();
            _diplomacy.ActorsInteraction(_owner, initiator, relation);
        }

        private void FromTargetState()
        {
            if (_deathCoroutine == null)
            {
                _stateMachine.ToPrevState();
                _signer.Invoke(this, TypeEvent.Change);
            }
        }
        #endregion
        
        private IEnumerator Death_Cn()
        {
            _unsubscribers.Unsubscribe();
            Removing();
            yield return _skin.Death();
            Dispose();
        }

        private void OnBuff(IPerk perk)
        {
            if (_abilities.AddPerk(perk) != 0)
                _signer.Invoke(this, TypeEvent.Change);
        }

        private void RedirectEvents(ReactiveEffect item, TypeEvent type) => _signer.Invoke(this, TypeEvent.Change);
        private void Signal() => _signer.Invoke(this, TypeEvent.Change);
    }
}
