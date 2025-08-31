using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public class DemonSkin : ActorSkin
    {
        public override void Init(Id<PlayerId> owner, Skills skills)
        {
            base.Init(skills, GetComponent<DemonSFX>());

        }
    }
}
