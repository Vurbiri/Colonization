using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
    sealed public partial class Demon
    {
        sealed public partial class BombStates : ADemonStates<BombSkin>
        {
            private SpecSkillSettings _specSkill;

            public BombStates(Demon actor, ActorSettings settings) : base(actor, settings) 
            {
                _skin.EventStart -= _stateMachine.ToDefaultState;
                _skin.EventStart += SpecSpawn;
            }

            public override void AddSpecSkillState(SpecSkillSettings specSkill, float runSpeed, float walkSpeed)
            {
                _specSkill = specSkill;
            }

            public override bool CanUseSpecSkill() => false;

            public override WaitSignal UseSpecSkill()
            {
                return null;
            }

            public override void Load()
            {
                _skin.EventStart -= SpecSpawn;
                _skin.EventStart += _stateMachine.ToDefaultState;
            }

            private void SpecSpawn()
            {
                _stateMachine.SetState(new ExplosionState(_specSkill, this), true);
                _specSkill = null;
            }
        }
    }
}
