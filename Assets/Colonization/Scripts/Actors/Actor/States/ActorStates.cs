using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.FSMSelectable;
using static Vurbiri.Colonization.Actors.Actor;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class AStates
    {
        protected readonly StateMachineSelectable _stateMachine = new();

        public abstract ActorSkin Skin { get; }
        public abstract bool IsAvailable { get; }
        public abstract bool IsNotDead { get; }

        [Impl(256)] public void ToDefault() => _stateMachine.ToDefaultState();

        [Impl(256)] public void Cancel() => _stateMachine.Cancel();
        [Impl(256)] public void Select() => _stateMachine.Select();
        [Impl(256)] public void Unselect(ISelectable newSelectable) => _stateMachine.Unselect(newSelectable);

        public abstract bool ToTarget();
        public abstract bool FromTarget();

        public abstract WaitSignal Move();
        public abstract WaitSignal UseSkill(int id);
        public abstract WaitSignal UseSpecSkill();
        public abstract WaitStateSource<DeathStage> Death();

        public virtual void Load() { }
    }

    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin> : AStates where TActor : Actor where TSkin : ActorSkin
        {
            protected readonly TActor _actor;
            protected readonly TSkin _skin;

            protected readonly TargetState _targetState = new();
            protected MoveState _moveState;
            protected ASkillState[] _skillState;
            protected DeathState _deathState;

            sealed public override ActorSkin Skin { [Impl(256)] get => _skin; }
            sealed public override bool IsNotDead { [Impl(256)] get => _deathState == null; }

            protected AStates(TActor actor, ActorSettings settings)
            {
                _actor = actor;
                _skin = settings.InstantiateSkin<TSkin>(actor._owner, actor._thisTransform);

                #region Bounds
                var bounds = _skin.Bounds;
                actor._thisCollider.size = bounds.size;
                actor._thisCollider.center = bounds.center;
                actor._extentsZ = bounds.extents.z;
                #endregion

                _stateMachine.AssignDefaultState(new IdleState(this));
                settings.Skills.CreateStates(this);

                _skin.EventStart += _stateMachine.ToDefaultState;
            }

            [Impl(256)] sealed public override WaitSignal Move()
            {
                _stateMachine.SetState(_moveState, true);
                return _moveState.signal;
            }

            [Impl(256)] sealed public override WaitSignal UseSkill(int id)
            {
                _stateMachine.SetState(_skillState[id], true);
                return _skillState[id].signal;
            }

            [Impl(256)] sealed public override WaitStateSource<DeathStage> Death()
            {
                _stateMachine.ForceSetState(_deathState = new(this), true);
                return _deathState.stage;
            }

            [Impl(256)] sealed public override bool ToTarget() => _stateMachine.SetState(_targetState, true);
            [Impl(256)] sealed public override bool FromTarget() => _deathState == null && _stateMachine.GetOutToPrevState(_targetState);

            public abstract void AddSpecSkillState(SpecSkillSettings specSkill);

            [Impl(256)] public void AddMoveState(float speed) => _moveState = new(speed, this);
            [Impl(256)] public void SetCountState(int count) => _skillState = new ASkillState[count];
            [Impl(256)] public void AddSkillState(SkillSettings skill, float speedRun, int id) => _skillState[id] = ASkillState.Create(skill, speedRun, id, this);
        }
    }
}
