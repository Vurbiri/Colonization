using System.Collections;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
	public class DemonSFX : ActorSFX
    {
        protected string _specSFX;

        public void Init(ReadOnlyArray<string> hitSFX, string specSFX)
        {
            InitInternal(hitSFX);
            _specSFX = specSFX;
        }

        public IEnumerator Spec(ActorSkin target) => GameContainer.HitSFX.Hit(_specSFX, this, target);
    }
}
