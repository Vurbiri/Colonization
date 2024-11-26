//Assets\Colonization\Scripts\Actors\Skin\ActorSkin.cs
using System;
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour, IDisposable
    {
        [SerializeField] private RFloat _timeSwitchIdle = new(10f, 30f);
        [SerializeField] private ASFX _sfx;
        [SerializeField] private Animator _animator;

        #region CONST
        private const string T_IDLE = "tIdleAdv", B_IDLE = "bIdle";
        private const string B_MOVE = "bMove", B_RUN = "bRun", B_BLOCK = "bBlock";
        private const string T_REACT = "tReact", T_DEATH = "tDeath";
        private static readonly string[] T_SKILLS = { "bSkill_01", "bSkill_02", "bSkill_03", "bSkill_04" };
        public const int COUNT_SKILLS = 4;
        #endregion

        private readonly StateMachine _stateMachine = new();
        private int _idBoolState = 0;

        private BoolSwitchState _moveState, _runState, _blockState;
        private readonly BoolSwitchState[] _skillStates = new BoolSwitchState[COUNT_SKILLS];
        private ATriggerSwitchState _reactState, _deathState;

        public event Action EventStart;

        private void Start()
        {
            _stateMachine.SetDefaultState(new IdleState(this));

            _moveState  = CreateBoolState(B_MOVE);
            _runState   = CreateBoolState(B_RUN);
            _blockState = CreateBoolState(B_BLOCK);

            _reactState = new ReactState(this);
            _deathState = new DeathState(this);

            for (int i = 0; i < COUNT_SKILLS; i++)
                _skillStates[i] = CreateBoolState(T_SKILLS[i]);

            _animator.GetBehaviour<SpawnBehaviour>().EventExit += EventStart;
        }

        public void Idle() => _stateMachine.ToDefaultState();

        public void Block() => _stateMachine.SetState(_blockState);

        public void Move() => _stateMachine.SetState(_moveState);

        public void Run() => _stateMachine.SetState(_runState);

        public void Skill(int index) => _stateMachine.SetState(_skillStates[index]);

        public WaitActivate React()
        {
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
            _stateMachine.Dispose();
            for (int i = 0; i < COUNT_SKILLS; i++)
                _skillStates[i].Dispose();
            _moveState.Dispose();
            _runState.Dispose();
            _blockState.Dispose();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_animator == null)
                _animator = GetComponent<Animator>();
            if (_sfx == null)
                _sfx = GetComponent<ASFX>();


        }
#endif
    }
}
