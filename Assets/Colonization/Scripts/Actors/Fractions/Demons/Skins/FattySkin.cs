using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class FattySkin : ADemonSkin
    {
        private JumpState _jumpState;

        public override void Init(Id<PlayerId> owner, Skills skills)
        {
            var sfx = GetComponent<DemonSFX>();
            sfx.Init(skills.HitSfxNames, skills.Spec.SFXName);

            _jumpState = new(this, sfx, skills.Spec.Timing);

            base.InitInternal(skills.Timings, sfx);
        }

        public WaitSignal SpecAttack()
        {
            _stateMachine.SetState(_jumpState);
            return _jumpState.signal.Restart();
        }
    }
}
