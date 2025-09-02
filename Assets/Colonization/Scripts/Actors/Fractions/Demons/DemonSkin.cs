using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public class DemonSkin : ActorSkin
    {
        public override void Init(Id<PlayerId> owner, Skills skills)
        {
            var sfx = GetComponent<DemonSFX>();
            sfx.Init(skills.HitSfxNames, skills.Spec.SFXName);

            base.InitInternal(skills.Timings, sfx);
        }
    }
}
