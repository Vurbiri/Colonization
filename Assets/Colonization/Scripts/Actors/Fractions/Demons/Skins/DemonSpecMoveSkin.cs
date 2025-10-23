namespace Vurbiri.Colonization
{
	sealed public partial class DemonSpecMoveSkin : ADemonSkin
    {
        private SpecMoveState _specMoveState;

        public override void Init(Id<PlayerId> owner, Skills skills)
        {
            var sfx = GetComponent<DemonSFX>();
            sfx.Init(skills.HitSfxNames, skills.Spec.SFXName);

            base.InitInternal(skills.Timings, sfx);

            _specMoveState = new(this, sfx);
        }

        public WaitSignal SpecMove()
        {
            _stateMachine.SetState(_specMoveState);
            return _specMoveState.signal.Restart();
        }
    }
}
