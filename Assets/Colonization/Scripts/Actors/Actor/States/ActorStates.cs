using Vurbiri.Colonization.FSMSelectable;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract partial class Actor
    {
        public abstract class Actions
        {
            public abstract bool CanUseSkill(int id);
            public abstract bool CanUseSpecSkill();

            public abstract SkillCode GetSkillCode(int id);

            public abstract WaitSignal Move();
            public abstract WaitSignal UseSkill(int id);
            public abstract WaitSignal UseSpecSkill();
            public abstract WaitStateSource<DeathStage> Death();
        }
        //============================================================================
        public abstract class AStates : Actions
        {
            private readonly TargetState _targetState = new();
            protected readonly StateMachineSelectable _stateMachine = new();
            protected int _skillsCount;

            public int SkillsCount { [Impl(256)] get => _skillsCount; }
            public bool IsDefault { [Impl(256)] get => _stateMachine.IsDefaultState; }

            public abstract ActorSkin Skin { get; }
            public abstract ActorSFX SFX { get; }
            public abstract bool IsAvailable { get; }

            [Impl(256)] public void ToDefault() => _stateMachine.ToDefaultState();

            [Impl(256)] public void Cancel() => _stateMachine.Cancel();
            [Impl(256)] public void Select(MouseButton button) => _stateMachine.Select(button);
            [Impl(256)] public void Unselect(ISelectable newSelectable) => _stateMachine.Unselect(newSelectable);

            [Impl(256)] public bool ToTarget() => _stateMachine.SetState(_targetState, true);
            [Impl(256)] public bool FromTarget() => _stateMachine.GetOutToPrevState(_targetState);

            public virtual void Load() { }
        }
        //============================================================================
        public abstract partial class AStates<TActor, TSkin> : AStates where TActor : Actor where TSkin : ActorSkin
        {
            protected readonly TActor _actor;
            protected readonly TSkin _skin;
            
            protected MoveState _moveState;
            protected ASkillState[] _skillState;
            protected DeathState _deathState;

            sealed public override ActorSkin Skin => _skin;
            sealed public override ActorSFX SFX => _skin.SFX;

            protected AStates(TActor actor, ActorSettings settings)
            {
                _actor = actor;
                _skin = settings.InstantiateSkin<TSkin>(actor._owner, actor._thisTransform);

                _stateMachine.AssignDefaultState(new IdleState(this));
                settings.Skills.CreateStates(this);

                _skin.EventStart += _stateMachine.ToDefaultState;
            }

            sealed public override bool CanUseSkill(int id) => _skillState[id].CanUse;

            sealed public override SkillCode GetSkillCode(int id) => _skillState[id].code;

            sealed public override WaitSignal Move()
            {
                _stateMachine.SetState(_moveState, true);
                return _moveState.signal.Restart();
            }
            
            sealed public override WaitSignal UseSkill(int id)
            {
                _stateMachine.SetState(_skillState[id], true);
                return _skillState[id].signal.Restart();
            }

            sealed public override WaitStateSource<DeathStage> Death()
            {
                _stateMachine.ForceSetState(_deathState = new(this), true);
                return _deathState.stage;
            }

            [Impl(256)] public void AddMoveState(float speed) => _moveState = new(speed, this);

            [Impl(256)] public void SetCountState(int count) => _skillState = new ASkillState[_skillsCount = count];
            [Impl(256)] public void AddSkillState(SkillSettings skill, float speedRun, int id) => _skillState[id] = ASkillState.Create(skill, speedRun, id, this);
            
            public abstract void AddSpecSkillState(SpecSkillSettings specSkill, float runSpeed, float walkSpeed);
        }
    }
}
