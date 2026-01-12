using System;
using UnityEngine;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	[RequireComponent(typeof(BoxCollider))]
	public abstract partial class Actor : MonoBehaviour, IReactiveItem<Actor>, IInteractable, IEquatable<ActorCode>
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
		private int _force;
		private bool _isPersonTurn;
		private bool _zealCharge;

		protected readonly VAction<Actor, TypeEvent> _eventChanged = new();

		#region  --------------- Abilities ---------------
		private AbilitiesSet<ActorAbilityId> _abilities;
		private SubAbility<ActorAbilityId> _HP;
		private SubAbility<ActorAbilityId> _AP;
		private BooleanAbility<ActorAbilityId> _move;
		private ChanceAbility<ActorAbilityId> _profitMain;
		private ChanceAbility<ActorAbilityId> _profitAdv;
		#endregion

		private Hexagon _currentHex;
		private BoxCollider _thisCollider;
		private EffectsSet _effects;
		private States _states;

		private float _zSize;

		private readonly ReactiveBool _interactable = new(false);
		private readonly ReactiveBool _canCancel = new(false);

		private Subscription _subscription;
		#endregion

		#region ================== Properties ========================
		public int Id { [Impl(256)] get => _id; }
		public Id<ActorTypeId> TypeId { [Impl(256)] get => _typeId; }
		public bool IsWarrior { [Impl(256)] get; [Impl(256)] private set; }
		public bool IsDemon { [Impl(256)] get; [Impl(256)] private set; }
		public Id<PlayerId> Owner { [Impl(256)] get => _owner; }
		public Hexagon Hexagon { [Impl(256)] get => _currentHex; }
		public int CurrentAP { [Impl(256)] get => _AP.Value; }
		public bool IsMaxAP { [Impl(256)] get => _AP.IsMax; }
		public bool CanUseSkills { [Impl(256)] get => _states.IsDefault & _isPersonTurn; }
		public int MaxHP { [Impl(256)] get => _HP.MaxValue; }
		public int CurrentHP { [Impl(256)] get => _HP.Value; }
		public int PercentHP { [Impl(256)] get => _HP.Percent; }
		public bool IsFullHP { [Impl(256)] get => _HP.IsMax; }
		public bool IsWounded { [Impl(256)] get => _HP.IsNotMax; }
		public bool IsDead { [Impl(256)] get => _HP.Value <= 0; }
		public bool ZealCharge { [Impl(256)] get => _zealCharge; [Impl(256)] set { _zealCharge = value; ChangeSignal(); } }
		public int CurrentForce { [Impl(256)] get => (_force * _HP.Percent << (IsWallDefenceEffect() ? 1 : 0) ) / 100 ; }
		public Transform Transform { [Impl(256)] get => _thisTransform; }
		public ActorSkin Skin { [Impl(256)] get => _states.Skin; }
		public Actions Action { [Impl(256)] get => _states; }
		public ReactiveEffects Effects { [Impl(256)] get => _effects; }
		public ReadOnlyAbilities<ActorAbilityId> Abilities { [Impl(256)] get => _abilities; }
		public ReturnSignal IsMainProfit => _profitMain.Next() ? _states.Skin.MainProfit(_isPersonTurn) : false;
		public ReturnSignal IsAdvProfit => _profitAdv.Next() ? _states.Skin.AdvProfit(_isPersonTurn) : false;

		#endregion

		#region  ================== IInteractable =====================
		public Vector3 Position { [Impl(256)] get => _thisTransform.position; }
		public Reactive<bool> CanCancel => _canCancel;
		public Reactive<bool> InteractableReactive => _interactable;
		public bool Interactable { [Impl(256)] get => _interactable.Value; [Impl(256)] set => _thisCollider.enabled = (_interactable.Value = value) & _isPersonTurn; }
	   
		public void Select(MouseButton button) => _states.Select(button);
		public void Unselect(ISelectable newSelectable) => _states.Unselect(newSelectable);
		public void Cancel() => _states.Cancel();

		[Impl(256)] public void SetLeftSelectable()
		{
			_thisCollider.enabled = true;
			gameObject.layer = Layers.SelectableLeft;
		}
		[Impl(256)] public void ResetLeftSelectable()
		{
			_thisCollider.enabled = _isPersonTurn;
			gameObject.layer = Layers.SelectableRight;
		}
		#endregion
		
		#region ================== IReactiveItem ============================
		public int Index { [Impl(256)] get; [Impl(256)] private set; }
		public ActorCode Code { [Impl(256)] get; [Impl(256)] private set; }

		public void Adding(Action<Actor, TypeEvent> action, int index)
		{
			Index = index;
			Code = new(_owner.Value, index);

			action(this, TypeEvent.Add);
			_eventChanged.Add(action);
		}

		public Subscription Subscribe(Action<Actor, TypeEvent> action, bool instantGetValue = true)
		{
			if (instantGetValue)
				action(this, TypeEvent.Subscribe);
			return _eventChanged.Add(action);
		}
		public void Unsubscribe(Action<Actor, TypeEvent> action) => _eventChanged.Remove(action);
		public void UnsubscribeAll() => _eventChanged.Clear();

		public void Removing()
		{
			_currentHex.ActorExit();
			_subscription?.Dispose();

			_eventChanged.Invoke(this, TypeEvent.Remove);
			_eventChanged.Clear();

			_effects.Dispose();
		}

		public void Dispose() { }
		#endregion

		#region ================== Setup ============================
		protected abstract States StatesCreate(ActorSettings settings);

		public void Setup(ActorSettings settings, ActorInitData initData, Hexagon startHex, bool enable)
		{
			_thisTransform = GetComponent<Transform>();
			_thisCollider = GetComponent<BoxCollider>();

			_typeId = settings.TypeId;
			_id = settings.Id;
			_force = settings.Force;
			_owner = initData.owner;
			_currentHex = startHex;

			IsWarrior = _typeId == ActorTypeId.Warrior;
			IsDemon   = _typeId == ActorTypeId.Demon;

			#region Abilities
			_abilities = settings.GetAbilities();

			_HP         = _abilities.ReplaceToSub(ActorAbilityId.CurrentHP, ActorAbilityId.MaxHP, ActorAbilityId.HPPerTurn);
			_AP         = _abilities.ReplaceToSub(ActorAbilityId.CurrentAP, ActorAbilityId.MaxAP, ActorAbilityId.APPerTurn);
			_move       = _abilities.ReplaceToBoolean(ActorAbilityId.IsMove);
			_profitMain = _abilities.ReplaceToChance(ActorAbilityId.ProfitMain, _AP);
			_profitAdv  = _abilities.ReplaceToChance(ActorAbilityId.ProfitAdv, _AP);

			for (int i = 0; i < initData.buffs.Count; ++i)
				_subscription += initData.buffs[i].Subscribe(OnBuff);
			#endregion

			#region Effects
			_effects = new(_abilities);

			_HP.Subscribe(OnDeath, false);
			_effects.Subscribe(RedirectEvents);
			#endregion

			_states = StatesCreate(settings);

			_zSize = _states.Skin.SetupCollider(_thisCollider);

			_thisTransform.SetLocalPositionAndRotation(_currentHex.Position, HEX.ROTATIONS[GetNearGroundHexOffset(_currentHex)]);
			_currentHex.ActorEnter(this);

			_subscription += GameContainer.GameEvents.Subscribe(OnGameMode);

            gameObject.SetActive(enable);

            #region Local GetNearGroundHexOffset(..)
            //===================================================
            static Key GetNearGroundHexOffset(Hexagon hexagon)
			{
				var neighbors = hexagon.Neighbors;
				for (int i = 0; i < neighbors.Count; ++i)
					 if (neighbors[i].IsWater)
						return hexagon.Key - neighbors[i].Key;

				return HEX.NEAR.Rand();
			}
			#endregion
		}

		public void SetLoadData(ActorLoadData data)
		{
			_HP.Set(data.state.currentHP);
			_AP.Set(data.state.currentAP);
			_move.Set(data.state.move);

			_zealCharge = data.state.zealCharge;

			for (int i = data.effects.Length - 1; i >= 0; --i)
				_effects.Add(data.effects[i]);

			_states.Load();
        }
		#endregion

		#region ================== Utilities ============================
		[Impl(256)] public bool IsSkillApplied(SkillCode skillCode) => _effects.Contains(skillCode);
		[Impl(256)] public bool IsCanApplySkill(Id<PlayerId> id, Relation typeAction)
		{
			return _states.IsAvailable && GameContainer.Diplomacy.IsCanActorsInteraction(id, _owner, typeAction);
		}

		#region ---------------- Combat ----------------
		public bool IsInCombat() => _currentHex.IsEnemyNear(_owner);
		public bool IsGuard()
		{
			var crossroads = _currentHex.Crossroads;
			for (int i = 0; i < HEX.VERTICES; ++i)
				if (crossroads[i].TryGetOwnerColony(out Id<PlayerId> owner) && owner == _owner)
					return true;
			return false;
		}
		public int GetCurrentForceNearEnemies()
		{
			int force = 0;
			var neighbors = _currentHex.Neighbors;
			for (int i = 0; i < HEX.SIDES; ++i)
				if (neighbors[i].TryGetEnemy(_owner, out Actor enemy))
					force += enemy.CurrentForce;
			return force;
		}
		#endregion

		#region ---------------- Diplomacy ----------------
		[Impl(256)] public bool IsFriend(Id<PlayerId> id) => GameContainer.Diplomacy.IsFriend(_owner, id);
		[Impl(256)] public bool IsGreatFriend(Id<PlayerId> id) => GameContainer.Diplomacy.IsGreatFriend(_owner, id);
		[Impl(256)] public bool IsEnemy(Id<PlayerId> id) => GameContainer.Diplomacy.IsEnemy(_owner, id);
		[Impl(256)] public bool IsGreatEnemy(Id<PlayerId> id) => GameContainer.Diplomacy.IsGreatEnemy(_owner, id);
		#endregion

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

			if(delta != 0 & _HP.IsTrue)
				ChangeSignal();

			return delta;
		}
		[Impl(256)] public void ClearEffects(int duration, Id<ClearEffectsId> type) => _effects.Degrade(duration, type);
		#endregion

		#region ================== Start/End turn ========================
		public void StatesUpdate()
		{
			_HP.Next();
			_AP.Next();
			_move.On();

			ChangeSignal();
		}
		public void EffectsUpdate(int defense)
		{
			_effects.Next();
			_effects.Add(WallEffectFactory.Create(defense));

			_states.ToDefault();
		}
		[Impl(256)] public void EffectsUpdate() => EffectsUpdate(_currentHex.GetMaxDefense());
		#endregion

		#region ================== WallDefence ============================
		[Impl(256)] public void AddWallDefenceEffect(int defense) => _effects.Add(WallEffectFactory.Create(defense));
		[Impl(256)] public void RemoveWallDefenceEffect() => _effects.Remove(WallEffectFactory.WallEffectCode);
		[Impl(256)] public bool IsWallDefenceEffect() => _effects.Contains(WallEffectFactory.WallEffectCode);
		#endregion

		#region ================== Target ============================
		public bool ToTargetState(Id<PlayerId> initiator, Relation relation)
		{
			bool isSet = GameContainer.Diplomacy.IsCanActorsInteraction(initiator, _owner, relation) && _states.ToTarget();
			if (isSet)
				GameContainer.Diplomacy.ActorsInteraction(_owner, initiator, relation, IsInCombat());
			return isSet;
		}
		public void FromTargetState()
		{
			if (_HP.IsTrue && _states.FromTarget())
				ChangeSignal();
		}
		#endregion

		#region ================== Private Utilities ============================
		private void OnBuff(IPerk perk)
		{
			if (_abilities.AddPerk(perk) != 0)
				_eventChanged.Invoke(this, TypeEvent.Change);
		}
		private void OnGameMode(Id<GameModeId> gameMode, TurnQueue turn)
		{
			bool isPlay = gameMode == GameModeId.Play && turn.currentId == _owner;

			_isPersonTurn = isPlay & turn.isPerson;
			Interactable = isPlay;
		}

		private void OnDeath(int hp) { if (hp <= 0) _states.Death(); }

		private void RedirectEvents(ReactiveEffect item, TypeEvent type) => _eventChanged.Invoke(this, TypeEvent.Change);
		
		[Impl(256)] private void ChangeSignal() => _eventChanged.Invoke(this, TypeEvent.Change);
		#endregion

		#region ================== Equals ============================
		[Impl(256)] public bool Equals(ISelectable other) => System.Object.ReferenceEquals(this, other);
		[Impl(256)] public bool Equals(Actor other) => other == this;
		[Impl(256)] public bool Equals(ActorCode code) => _owner == code.owner && Index == code.index;
		#endregion

		[Impl(256)] public static implicit operator ActorCode(Actor self) => self.Code;

#if UNITY_EDITOR
		public override string ToString() => (IsWarrior ? WarriorId.Names_Ed : DemonId.Names_Ed)[_id];
#else
		public override string ToString() => (IsWarrior ? "Warrior" : "Demon").Concat("_", _id.ToStr());
#endif
	}
}
