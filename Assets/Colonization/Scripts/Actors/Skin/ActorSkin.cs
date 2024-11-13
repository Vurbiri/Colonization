using System;
using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour
    {
        [SerializeField] private RFloat _timeSwitchIdle = new(10f, 30f);

        #region CONST
        private const string T_IDLE = "tIdle", B_IDLE = "bIdle";
        private const string B_MOVE = "bMove", B_RUN = "bRun";
        private static readonly string[] T_SKILLS = { "tSkill_01", "tSkill_02" };
        private const int COUNT_ATTACKS = 2;
        #endregion

        private Animator _animator;
        private readonly StateMachine _stateMachine = new();
        private int _idBoolState = 0, _idTriggerState = 0;

        private BoolSwitchState _moveState, _runState;
        private readonly TriggerSwitchState[] _skillStates = new TriggerSwitchState[COUNT_ATTACKS];

        public event Action EventStart;

        private void Start()
        {
            _animator = GetComponent<Animator>();

            _stateMachine.AddState(new IdleState(this));
            _stateMachine.SetDefaultState<IdleState>();

            _moveState       = CreateBoolState(B_MOVE);
            _runState         = CreateBoolState(B_RUN);

            for (int i = 0; i < COUNT_ATTACKS; i++)
                _skillStates[i] = CreateTriggerState(T_SKILLS[i]);

            _animator.GetBehaviour<SpawnBehaviour>().EventExitSpawn += EventStart;
        }

        public void Idle() => _stateMachine.SetState<IdleState>();

        public void Move() => _stateMachine.SetState(_moveState);

        public void Run() => _stateMachine.SetState(_runState);

        public void Skill(int index) => _stateMachine.SetState(_skillStates[index]);
        

        public void Default() => _stateMachine.Default();


        private BoolSwitchState CreateBoolState(string nameParam) => new(nameParam, this, _idBoolState++);
        private TriggerSwitchState CreateTriggerState(string nameParam) => new(nameParam, this, _idTriggerState++);
    }
}
