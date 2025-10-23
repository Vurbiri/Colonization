namespace Vurbiri.Colonization
{
    sealed public partial class BossSkin : ADemonSkin
    {
        private EatState _eatState;

        public override void Init(Id<PlayerId> owner, Skills skills)
        {
            var sfx = GetComponent<BossSFX>();
            sfx.Init(skills.HitSfxNames, skills.Spec.SFXName);

            _eatState = new(this, sfx, skills.Spec.Timing);

            base.InitInternal(skills.Timings, sfx);
        }

        public WaitSignal SpecSkill()
        {
            _stateMachine.SetState(_eatState);
            return _eatState.signal.Restart();
        }
    }
}
