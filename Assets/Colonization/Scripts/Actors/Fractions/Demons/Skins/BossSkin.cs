using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class BossSkin : ADemonSkin
    {
        public override void Init(Id<PlayerId> owner, Skills skills)
        {
            var sfx = GetComponent<BossSFX>();
            sfx.Init(skills.HitSfxNames, skills.Spec.SFXName);

            base.InitInternal(skills.Timings, sfx);
        }
    }
}
