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
    public abstract partial class Actor : AReactiveItemMono<Actor>, ISelectable, ICancel, IDisposable, IPositionable
    {
        #region Fields
        protected int _typeId;
        protected int _id;
        protected Id<PlayerId> _owner;

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
        protected Collider _thisCollider;
        protected Diplomacy _diplomacy;
        protected GameplayTriggerBus _triggerBus;
        protected float _extentsZ;

        protected EffectsSet _effects;

        protected StateMachineSelectable _stateMachine;
        protected BlockState _blockState;

        protected bool _isPlayerTurn;

        protected RBool _canCancel = new(false);

        protected Coroutine _deathCoroutine;
        protected Unsubscribers _unsubscribers = new();
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
        public IReadOnlyAbilities<ActorAbilityId> Abilities => _abilities;
        public IReactiveValue<bool> CanCancel => _canCancel;
        public bool IsMainProfit => _profitMain.Next();
        public bool IsAdvProfit => _profitAdv.Next();
        #endregion

        #region States
        public void Move() => _stateMachine.SetState<MoveState>();
        public void Block() => _stateMachine.SetState(_blockState);
        public void UseSkill(int id) => _stateMachine.SetState<ASkillState>(id);
        public void Cancel() => _stateMachine.Cancel();
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

            if(delta != 0)
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

            _isPlayerTurn = _thisCollider.enabled = false;
        }
        public void EffectsUpdate(int defense)
        {
            _isPlayerTurn = _thisCollider.enabled = _owner == PlayerId.Player;

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

        #region Collider
        public void Collider(bool enabled) => _thisCollider.enabled = enabled;
        private void ColliderEnable() => _thisCollider.enabled = _isPlayerTurn;
        private void ColliderDisable() => _thisCollider.enabled = false;
        #endregion

        #region ISelectable
        public void Select() => _stateMachine.Select();
        public void Unselect(ISelectable newSelectable) => _stateMachine.Unselect(newSelectable);
        #endregion

        sealed public override bool Equals(Actor other) => System.Object.ReferenceEquals(this, other);
        sealed public override void Dispose()
        {
            _currentHex.ExitActor();

            _skin.Dispose();
            _stateMachine.Dispose();
            _effects.Dispose();

            Destroy(gameObject);
        }

        #region BecomeTarget
        private void BecomeTargetStart(Id<PlayerId> initiator, Relation relation)
        {
            _stateMachine.SetState<BecomeTargetState>();
            _diplomacy.ActorsInteraction(_owner, initiator, relation);
        }

        private void BecomeTargetEnd()
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
