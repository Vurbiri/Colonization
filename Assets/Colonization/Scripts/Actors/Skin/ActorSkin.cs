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
        private const string B_MOVE = "bMove", B_RUN_F = "bRunF", B_RUN_B = "bRunB";
        private const string T_ATTACK_01 = "tAttack_01", T_ATTACK_02 = "tAttack_02";
        #endregion

        private Animator _animator;
        private readonly StateMachine _stateMachine = new();
        private int _idBoolState = 0, _idTriggerState = 0;

        private BoolSwitchState _moveState, _runForwardState, _runBackState;
        private TriggerSwitchState _attackState;

        public event Action EventStart;

        private void Start()
        {
            _animator = GetComponent<Animator>();

            _stateMachine.AddState(new IdleState(this));
            _stateMachine.SetDefaultState<IdleState>();

            _moveState       = CreateBoolState(B_MOVE);
            _runForwardState = CreateBoolState(B_RUN_F);
            _runBackState    = CreateBoolState(B_RUN_B);

            _attackState = CreateTriggerState(T_ATTACK_01);

            _animator.GetBehaviour<SpawnBehaviour>().EventExitSpawn += EventStart;
        }

        public void Idle() => _stateMachine.SetState<IdleState>();

        public void Move() => _stateMachine.SetState(_moveState);

        public void RunForward() => _stateMachine.SetState(_runForwardState);

        public void Attack() => _stateMachine.SetState(_attackState);

        public void RunBack() => _stateMachine.SetState(_runBackState);

        public void Default() => _stateMachine.Default();


        private BoolSwitchState CreateBoolState(string nameParam) => new(nameParam, this, _idBoolState++);
        private TriggerSwitchState CreateTriggerState(string nameParam) => new(nameParam, this, _idTriggerState++);
    }
}
