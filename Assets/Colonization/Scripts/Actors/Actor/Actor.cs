using System;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract partial class Actor : AReactiveItemMono<Actor>, IInteractable, IDisposable
    {
        public enum DeathStage
        {
            None, Start, EndAnimation, End
        }

        #region Fields
        protected Id<ActorTypeId> _typeId;
        protected int _id;
        protected Id<PlayerId> _owner;
        private bool _isPersonTurn;

        #region Abilities
        protected AbilitiesSet<ActorAbilityId> _abilities;
        protected SubAbility<ActorAbilityId> _currentHP;
        protected SubAbility<ActorAbilityId> _currentAP;
        protected BooleanAbility<ActorAbilityId> _move;
        protected ChanceAbility<ActorAbilityId> _profitMain;
        protected ChanceAbility<ActorAbilityId> _profitAdv;
        #endregion

        protected Hexagon _currentHex;

        protected Transform _thisTransform;
        protected BoxCollider _thisCollider;

        protected EffectsSet _effects;

        protected AStates _states;

        private float _zSize;

        private readonly RBool _interactable = new(false);
        private readonly RBool _canCancel = new(false);

        private Subscription _subscription;
        #endregion

        #region Propirties
        public Id<ActorTypeId> TypeId => _typeId;
        public int Id => _id;
        public bool IsWarrior => _typeId == ActorTypeId.Warrior;
        public Id<PlayerId> Owner => _owner;
        public Hexagon Hexagon => _currentHex;
        public int ActionPoint => _currentAP.Value;
        public bool CanMove => _move.IsValue;
        public bool CanUseSkills => _states.IsDefault & _isPersonTurn;
        public int CurrentHP => _currentHP.Value;
        public bool IsWounded => _currentHP.IsNotMax;
        public bool IsDead => _currentHP.Value <= 0;
        public ActorSkin Skin => _states.Skin;
        public Actions Action => _states;
        public ReactiveEffects Effects => _effects;
        public ReadOnlyAbilities<ActorAbilityId> Abilities => _abilities;
        public bool IsMainProfit => _profitMain.Next();
        public bool IsAdvProfit => _profitAdv.Next();
        #endregion

        #region IInteractable
        public Vector3 Position => _thisTransform.position;
        public RBool CanCancel => _canCancel;
        public RBool InteractableReactive => _interactable;
        public bool Interactable { get => _interactable.Value; set => _thisCollider.enabled = _interactable.Value = _isPersonTurn & value; }
        public bool RaycastTarget { get => _thisCollider.enabled; set => _thisCollider.enabled = _isPersonTurn | value; }
        public bool IsPersonTurn { set => _isPersonTurn = value; }
       
        public void Select() => _states.Select();
        public void Unselect(ISelectable newSelectable) => _states.Unselect(newSelectable);
        public void Cancel() => _states.Cancel();
        #endregion

        #region Setup
        protected abstract AStates StatesCreate(ActorSettings settings);

        public void Setup(ActorSettings settings, ActorInitData initData, Hexagon startHex)
        {
            _thisTransform = transform;
            _thisCollider = GetComponent<BoxCollider>();

            _typeId = settings.TypeId;
            _id = settings.Id;
            _owner = initData.owner;
            _currentHex = startHex;
            IsPersonTurn = false;
            Interactable = false;

            #region Abilities
            _abilities = settings.Abilities;

            _currentHP  = _abilities.ReplaceToSub(ActorAbilityId.CurrentHP, ActorAbilityId.MaxHP, ActorAbilityId.HPPerTurn);
            _currentAP  = _abilities.ReplaceToSub(ActorAbilityId.CurrentAP, ActorAbilityId.MaxAP, ActorAbilityId.APPerTurn);
            _move       = _abilities.ReplaceToBoolean(ActorAbilityId.IsMove);
            _profitMain = _abilities.ReplaceToChance(ActorAbilityId.ProfitMain, _currentAP, _move);
            _profitAdv  = _abilities.ReplaceToChance(ActorAbilityId.ProfitAdv, _currentAP, _move);

            for (int i = 0; i < initData.buffs.Count; i++)
                _subscription += initData.buffs[i].Subscribe(OnBuff);
            #endregion

            #region Effects
            _effects = new(_abilities);

            _currentHP.Subscribe(OnDeath, false);
            _effects.Subscribe(RedirectEvents);
            #endregion

            _states = StatesCreate(settings);

            _zSize = _states.Skin.SetupCollider(_thisCollider);

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, CONST.ACTOR_ROTATIONS[GetNearGroundHexOffset(_currentHex)]);
            _currentHex.ActorEnter(this);
            gameObject.SetActive(true);

            #region Local GetNearGroundHexOffset(..)
            //===================================================
            static Key GetNearGroundHexOffset(Hexagon hexagon)
            {
                foreach (var neighbor in hexagon.Neighbors)
                    if (neighbor.IsWater)
                        return hexagon.Key - neighbor.Key;

                return HEX.NEAR.Rand();
            }
            #endregion
        }

        public void SetLoadData(ActorLoadData data)
        {
            _currentHP.Set(data.state.currentHP);
            _currentAP.Set(data.state.currentAP);
            _move.Set(data.state.move);

            for (int i = data.effects.Length - 1; i >= 0; i--)
                _effects.Add(data.effects[i]);

            _states.Load();
        }
        #endregion

        public bool IsUseSkill(SkillCode skillCode) => _effects.Contains(skillCode);
        public bool IsCanApplySkill(Id<PlayerId> id, Relation typeAction, out bool isFriendly)
        {
            return _states.IsAvailable & GameContainer.Diplomacy.IsCanActorsInteraction(id, _owner, typeAction, out isFriendly);
        }

        #region Effect
        public int AddEffect(ReactiveEffect effect) => _effects.Add(effect);
        public int ApplyEffect(IPerk effect)
        {
            int delta = _abilities.AddPerk(effect);

            if(delta != 0 & _currentHP.IsValue)
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

            _states.ToDefault();
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
            Interactable = _states.IsAvailable;
        }

        public bool Equals(ISelectable other) => System.Object.ReferenceEquals(this, other);
        sealed public override bool Equals(Actor other) => System.Object.ReferenceEquals(this, other);
        sealed public override void Removing()
        {
            _currentHex.ActorExit();
            _subscription?.Dispose();

            _eventChanged.Invoke(this, TypeEvent.Remove);
            _eventChanged.Clear();
            _index = -1;

            _effects.Dispose();
        }

        sealed public override void Dispose() { }

        #region Target
        private bool ToTargetState(Id<PlayerId> initiator, Relation relation)
        {
            bool isSet = GameContainer.Diplomacy.IsCanActorsInteraction(initiator, _owner, relation, out _) && _states.ToTarget();
            if(isSet)
                GameContainer.Diplomacy.ActorsInteraction(_owner, initiator, relation);
            return isSet;
        }

        private void FromTargetState()
        {
            if (_currentHP.IsValue && _states.FromTarget())
                _eventChanged.Invoke(this, TypeEvent.Change);
        }
        #endregion

        private void OnBuff(IPerk perk)
        {
            if (_abilities.AddPerk(perk) != 0)
                _eventChanged.Invoke(this, TypeEvent.Change);
        }

        private void OnDeath(int hp) { if (hp <= 0) _states.Death(); }

        private void RedirectEvents(ReactiveEffect item, TypeEvent type) => _eventChanged.Invoke(this, TypeEvent.Change);
        private void Signal() => _eventChanged.Invoke(this, TypeEvent.Change);
    }
}
