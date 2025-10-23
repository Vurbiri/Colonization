using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
    sealed public partial class BombSkin : ADemonSkin
    {
        private ExplosionState _explosionState;

        public override void Init(Id<PlayerId> owner, Skills skills)
        {
            var sfx = GetComponent<DemonSFX>();
            sfx.Init(skills.HitSfxNames, skills.Spec.SFXName);

            _explosionState = new(this, sfx, skills.Spec.Timing);

            base.InitInternal(skills.Timings, sfx);
        }

        public WaitSignal SpecSpawn()
        {
            var waitSignal = _explosionState.signal;
            _stateMachine.SetState(_explosionState);
            _explosionState = null;

            return waitSignal.Restart();
        }
    }
}
