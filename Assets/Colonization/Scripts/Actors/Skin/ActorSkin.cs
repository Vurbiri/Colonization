using UnityEngine;
using Vurbiri.FSM;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin : MonoBehaviour
    {
        [SerializeField] private RFloat _timeSwitchIdle = new(10f, 30f);
        
        private Animator _animator;
        private readonly StateMachine _machine = new();

        private readonly int _damageId = Animator.StringToHash("tDamage");
        private readonly int _attackId = Animator.StringToHash("tAttackHorizontal");

        private void Start()
        {
            _animator = GetComponent<Animator>();

            _machine.AddState(new IdleState(this, _machine));
            _machine.AddState(new MoveState(this, _machine));

            _machine.SetDefaultState<IdleState>();

            _animator.GetBehaviour<SpawnBehaviour>().EventExitSpawn += Idle;
        }

        public void Idle() => _machine.SetState<IdleState>();

        public void StartMove() => _machine.SetState<MoveState>();
        
        public void ResetStates() => _machine.Default();

    }
}
