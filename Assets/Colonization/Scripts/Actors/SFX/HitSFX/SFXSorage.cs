using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization.Actors
{
	public class SFXStorage
	{
		private readonly Dictionary<string, IHitSFX> _SFXs;

		public SFXStorage(Dictionary<string, IHitSFX> SFXs) => _SFXs = SFXs;

        public IEnumerator Hit(string name, ActorSFX user, ActorSkin target) => _SFXs[name].Hit(user, target);
    }
}
