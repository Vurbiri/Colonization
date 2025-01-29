//Assets\Colonization\Scripts\Actors\Skin\ActorSkin.cs
using System;
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour, IDisposable
    {
        [SerializeField] private AActorSFX _sfx;
        [SerializeField] private Animator _animator;
        [HideInInspector, SerializeField] TimingSkillSettings[] _timings;
        [HideInInspector, SerializeField] float _durationDeath;

        #region CONST
        private const string B_IDLE = "bIdle", B_MOVE = "bMove", B_RUN = "bRun", B_BLOCK = "bBlock", B_DEATH = "bDeath";
        private static readonly string[] B_SKILLS = { "bSkill_0", "bSkill_1", "bSkill_2", "bSkill_3" };
        private const string T_REACT = "tReact";
        #endregion

        private readonly StateMachine _stateMachine = new();
        private int _idBoolState = 0;

        private BoolSwitchState _moveState, _runState, _blockState;
        private SkillState[] _skillStates;
        private DeathState _deathState;
        private ReactState _reactState;

        public event Action EventStart;

        private void Start()
        {
            _stateMachine.SetDefaultState(CreateBoolState(B_IDLE));

            _moveState  = CreateBoolState(B_MOVE);
            _runState   = CreateBoolState(B_RUN);
            _blockState = CreateBoolState(B_BLOCK);

            _reactState = new(this);
            _deathState = new(this, _durationDeath);

            int count = _timings.Length;
            _skillStates = new SkillState[count];
            for (int i = 0; i < count; i++)
                _skillStates[i] = new(B_SKILLS[i], this, _timings[i], i);
            _timings = null;

            _animator.GetBehaviour<SpawnBehaviour>().EventExit += EventStart;
        }

        public virtual void Idle() => _stateMachine.ToDefaultState();

        public virtual void Block() => _stateMachine.SetState(_blockState);

        public virtual void Move() => _stateMachine.SetState(_moveState);

        public virtual void Run() => _stateMachine.SetState(_runState);

        public virtual WaitActivate Skill(int index, Transform target)
        {
            SkillState skill = _skillStates[index];
            skill.target = target;
            _stateMachine.SetState(skill);
            return skill.waitActivate;
        }

        public WaitActivate React()
        {
            _stateMachine.Update();
            _stateMachine.SetState(_reactState);
            return _reactState.waitActivate;
        }

        public WaitActivate Death()
        {
            _stateMachine.SetState(_deathState);
            return _deathState.waitActivate;
        }

        private BoolSwitchState CreateBoolState(string nameParam) => new(nameParam, this, _idBoolState++);

        public void Dispose()
        {
            _animator.GetBehaviour<SpawnBehaviour>().EventExit -= EventStart;

            _stateMachine.Dispose();
            for (int i = 0; i < _skillStates.Length; i++)
                _skillStates[i].Dispose();
            _moveState.Dispose();
            _runState.Dispose();
            _blockState.Dispose();
            _reactState.Dispose();
            _deathState.Dispose();
        }
    }
}
