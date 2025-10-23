using UnityEngine;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract partial class Actor : AReactiveItemMono<Actor>, IInteractable
    {
        public enum DeathStage
        {
            None, Start, EndAnimation, End
        }

        #region ================== Fields ============================
        protected int _id;
        protected Transform _thisTransform;

        private Id<ActorTypeId> _typeId;
        private Id<PlayerId> _owner;
        private bool _isPersonTurn;
        private bool _zealCharge;

        #region  --------------- Abilities ---------------
        private AbilitiesSet<ActorAbilityId> _abilities;
        private SubAbility<ActorAbilityId> _currentHP;
        private SubAbility<ActorAbilityId> _currentAP;
        private BooleanAbility<ActorAbilityId> _move;
        private ChanceAbility<ActorAbilityId> _profitMain;
        private ChanceAbility<ActorAbilityId> _profitAdv;
        #endregion

        private Hexagon _currentHex;
        private BoxCollider _thisCollider;
        private EffectsSet _effects;
        private AStates _states;

        private float _zSize;

        private readonly RBool _interactable = new(false);
        private readonly RBool _canCancel = new(false);

        private Subscription _subscription;
        #endregion

        #region ================== Properties ========================
        public Id<ActorTypeId> TypeId { [Impl(256)] get => _typeId; }
        public int Id { [Impl(256)] get => _id; }
        public bool IsWarrior { [Impl(256)] get => _typeId == ActorTypeId.Warrior; }
        public Id<PlayerId> Owner { [Impl(256)] get => _owner; }
        public Hexagon Hexagon { [Impl(256)] get => _currentHex; }
        public int ActionPoint { [Impl(256)] get => _currentAP.Value; }
        public bool CanMove { [Impl(256)] get => _move.IsTrue; }
        public bool CanUseSkills { [Impl(256)] get => _states.IsDefault & _isPersonTurn; }
        public int CurrentHP { [Impl(256)] get => _currentHP.Value; }
        public bool IsFullHP { [Impl(256)] get => _currentHP.IsMax; }
        public bool IsWounded { [Impl(256)] get => _currentHP.IsNotMax; }
        public bool IsDead { [Impl(256)] get => _currentHP.Value <= 0; }
        public bool ZealCharge { [Impl(256)] get => _zealCharge; [Impl(256)] set { _zealCharge = value; _eventChanged.Invoke(this, TypeEvent.Change); } }
        public ActorSkin Skin { [Impl(256)] get => _states.Skin; }
        public Actions Action { [Impl(256)] get => _states; }
        public ReactiveEffects Effects { [Impl(256)] get => _effects; }
        public ReadOnlyAbilities<ActorAbilityId> Abilities { [Impl(256)] get => _abilities; }
        public ReturnSignal IsMainProfit => _profitMain.Next() ? _states.Skin.MainProfit(_isPersonTurn) : false;
        public ReturnSignal IsAdvProfit => _profitAdv.Next() ? _states.Skin.AdvProfit(_isPersonTurn) : false;
        #endregion

        #region  ================== IInteractable =====================
        public Vector3 Position => _thisTransform.position;
        public ReactiveValue<bool> CanCancel => _canCancel;
        public ReactiveValue<bool> InteractableReactive => _interactable;
        public bool Interactable { get => _interactable.Value; set => _thisCollider.enabled = _interactable.Value = _isPersonTurn & value; }
        public bool RaycastTarget { get => _thisCollider.enabled; set => _thisCollider.enabled = _isPersonTurn | value; }
        public bool IsPersonTurn { set => _isPersonTurn = value; }
       
        public void Select() => _states.Select();
        public void Unselect(ISelectable newSelectable) => _states.Unselect(newSelectable);
        public void Cancel() => _states.Cancel();
        #endregion

        #region ================== Setup ============================
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

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, HEX.ROTATIONS[GetNearGroundHexOffset(_currentHex)]);
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

            _zealCharge = data.state.zealCharge;

            for (int i = data.effects.Length - 1; i >= 0; i--)
                _effects.Add(data.effects[i]);

            _states.Load();
        }
        #endregion

        #region ================== Utilities ============================
        [Impl(256)] public bool IsSkillApplied(SkillCode skillCode) => _effects.Contains(skillCode);
        [Impl(256)] public bool IsCanApplySkill(Id<PlayerId> id, Relation typeAction, out bool isFriendly)
        {
            return _states.IsAvailable & GameContainer.Diplomacy.IsCanActorsInteraction(id, _owner, typeAction, out isFriendly);
        }

        [Impl(256)] public bool IsEnemy(Id<PlayerId> id) => GameContainer.Diplomacy.IsEnemy(_owner, id);

        public bool IsInCombat()
        {
            foreach (var hex in _currentHex.Neighbors)
                if (hex.IsEnemy(_owner))
                    return true;
            return false;
        }

        #region ---------------- HexSwap ----------------
        [Impl(256)] public void SetHexagonSelectable()
        {
            _currentHex.SetSelectableForSwap();
            Interactable = false;
        }
        [Impl(256)] public void SetHexagonUnselectable()
        {
            _currentHex.SetUnselectableForSwap();
            Interactable = _states.IsAvailable;
        }
        #endregion
        #endregion

        #region ================== Effect ============================
        [Impl(256)] public int AddEffect(ReactiveEffect effect) => _effects.Add(effect);
        [Impl(256)] public int ApplyEffect(IPerk effect)
        {
            int delta = _abilities.AddPerk(effect);

            if(delta != 0 & _currentHP.IsTrue)
                _eventChanged.Invoke(this, TypeEvent.Change);

            return delta;
        }
        [Impl(256)] public void ClearEffects(int duration, Id<ClearEffectsId> type) => _effects.Degrade(duration, type);
        #endregion

        #region ================== Start/End turn ========================

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

        #region ================== WallDefence ============================
        [Impl(256)] public void AddWallDefenceEffect(int maxDefense) => _effects.Add(ReactiveEffectsFactory.CreateWallDefenceEffect(maxDefense));
        [Impl(256)] public void RemoveWallDefenceEffect() => _effects.Remove(ReactiveEffectsFactory.WallEffectCode);
        #endregion

        #region ================== ReactiveItem ============================
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
        #endregion

        #region ================== Target ============================
        public bool ToTargetState(Id<PlayerId> initiator, Relation relation)
        {
            bool isSet = GameContainer.Diplomacy.IsCanActorsInteraction(initiator, _owner, relation, out _) && _states.ToTarget();
            if(isSet)
                GameContainer.Diplomacy.ActorsInteraction(_owner, initiator, relation);
            return isSet;
        }
        public void FromTargetState()
        {
            if (_currentHP.IsTrue && _states.FromTarget())
                _eventChanged.Invoke(this, TypeEvent.Change);
        }
        #endregion

        #region ================== Private Utilities ============================
        private void OnBuff(IPerk perk)
        {
            if (_abilities.AddPerk(perk) != 0)
                _eventChanged.Invoke(this, TypeEvent.Change);
        }

        private void OnDeath(int hp) { if (hp <= 0) _states.Death(); }

        private void RedirectEvents(ReactiveEffect item, TypeEvent type) => _eventChanged.Invoke(this, TypeEvent.Change);
        private void Signal() => _eventChanged.Invoke(this, TypeEvent.Change);
        #endregion
    }
}
