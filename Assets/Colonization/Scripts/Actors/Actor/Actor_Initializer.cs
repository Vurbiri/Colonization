namespace Vurbiri.Colonization.Actors
{
    using System.Collections.Generic;
    using UnityEngine;
    using static CONST;

    public abstract partial class Actor
    {
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
            _effects.Subscribe(RedirectEvents);

            _skin.EventStart += _stateMachine.Default;

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, ACTOR_ROTATIONS[_currentHex.GetNearGroundHexOffset()]);
            _currentHex.EnterActor(this);

            Skills skills = settings.Skills;

            _stateMachine.AddState(new IdleState(this));
            _stateMachine.SetDefaultState<IdleState>();

            _stateMachine.AddState(skills.GetMoveSate(this));

            List<AttackState> attackStates = skills.GetAttackSates(this);
            _countAttackStates = attackStates.Count;
            for (int i = 0; i < _countAttackStates; i++)
                _stateMachine.AddState(attackStates[i]);

            _thisGO.SetActive(true);
        }

        public virtual void Init(ActorSettings settings, int owner, Hexagon startHex, int[][] data, GameplayEventBus eventBus)
        {
            Init(settings, owner, startHex, eventBus);

            _currentHP = data[1][1];
            _currentActionPoint = data[1][2];
            for (int i = 2; i < data.Length; i++)
                _effects.Add(new(data[i]));
        }
    }
}
