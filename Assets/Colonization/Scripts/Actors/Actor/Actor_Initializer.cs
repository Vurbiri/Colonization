namespace Vurbiri.Colonization.Actors
{
    using UnityEngine;
    using static CONST;

    public abstract partial class Actor
    {
        public virtual void Init(ActorSettings settings, int owner, Hexagon startHex, GameplayEventBus eventBus)
        {
            _id = settings.Id;
            _owner = owner;
            _abilities = settings.Abilities;
            _skin = settings.InstantiateActorSkin(transform);
            _currentHex = startHex;
            _eventBus = eventBus;

            _currentHP = _abilities.GetAbility(ActorAbilityId.CurrentHP);
            _currentHP.Clamp = CurrentHPCamp;
            _currentAP = _abilities.GetAbility(ActorAbilityId.CurrentAP);
            _currentAP.Clamp = CurrentAPCamp;
            _isMove = _abilities.GetAbility(ActorAbilityId.IsMove);

            _thisTransform = transform;
            _extentsZ = GetComponent<BoxCollider>().bounds.extents.z;
            _effects.Subscribe(RedirectEvents);

            _skin.EventStart += _stateMachine.Default;

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, ACTOR_ROTATIONS[_currentHex.GetNearGroundHexOffset()]);
            _currentHex.EnterActor(this);

            Skills skills = settings.Skills;

            _stateMachine.AddState(new IdleState(this));
            _stateMachine.SetDefaultState<IdleState>();

            _stateMachine.AddState(skills.GetMoveSate(this));

            _skillStates = skills.GetSkillSates(this);

            gameObject.SetActive(true);
        }

        public virtual void Init(ActorSettings settings, int owner, Hexagon startHex, ActorLoadData data, GameplayEventBus eventBus)
        {
            Init(settings, owner, startHex, eventBus);

            _currentHP.BaseValue = data.currentHP;
            _currentAP.BaseValue = data.currentAP;
            _isMove.BaseValue    = data.move;

            int count = data.effects.Length;
            for (int i = 0; i < count; i++)
                _effects.Add(data.effects[i]);
        }
    }
}
