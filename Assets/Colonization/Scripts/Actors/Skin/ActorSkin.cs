//Assets\Colonization\Scripts\Actors\Skin\ActorSkin.cs
using System;
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour, IDisposable
    {
        [SerializeField] private RFloat _timeSwitchIdle = new(10f, 30f);

        #region CONST
        private const string T_IDLE = "tIdleAdv", B_IDLE = "bIdle";
        private const string B_MOVE = "bMove", B_RUN = "bRun", B_BLOCK = "bBlock";
        private const string T_REACT = "tReact";
        private static readonly string[] T_SKILLS = { "bSkill_01", "bSkill_02", "bSkill_03" };
        public const int COUNT_SKILLS = 3;
        #endregion

        private Animator _animator;
        private readonly StateMachine _stateMachine = new();
        private int _idBoolState = 0, _idTriggerState = 0;

        private BoolSwitchState _moveState, _runState, _blockState;
        private readonly BoolSwitchState[] _skillStates = new BoolSwitchState[COUNT_SKILLS];

        public event Action EventStart;

        private void Start()
        {
            _animator = GetComponent<Animator>();

            _stateMachine.AddState(new IdleState(this));
            _stateMachine.SetDefaultState<IdleState>();
            _stateMachine.AddState(new ReactState(this));

            _moveState  = CreateBoolState(B_MOVE);
            _runState   = CreateBoolState(B_RUN);
            _blockState = CreateBoolState(B_BLOCK);

            for (int i = 0; i < COUNT_SKILLS; i++)
                _skillStates[i] = CreateBoolState(T_SKILLS[i]);

            _animator.GetBehaviour<SpawnBehaviour>().EventExit += EventStart;
        }

        public void Idle() => _stateMachine.SetState<IdleState>();

        public void Block() => _stateMachine.SetState(_blockState);

        public void Move() => _stateMachine.SetState(_moveState);

        public void Run() => _stateMachine.SetState(_runState);

        public void Skill(int index) => _stateMachine.SetState(_skillStates[index]);

        public void React() => _stateMachine.SetState<ReactState>();
        
        public void Death() { }

        public void ToDefault() => _stateMachine.ToDefaultState();


        private BoolSwitchState CreateBoolState(string nameParam) => new(nameParam, this, _idBoolState++);
        private TriggerSwitchState CreateTriggerState(string nameParam) => new(nameParam, this, _idTriggerState++);

        public void Dispose()
        {
            _stateMachine.Dispose();
            for (int i = 0; i < COUNT_SKILLS; i++)
                _skillStates[i].Dispose();
            _moveState.Dispose();
            _runState.Dispose();
            _blockState.Dispose();
        }
    }
}
