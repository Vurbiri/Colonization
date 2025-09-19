using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization.Actors
{
	public class SFXStorage
	{
		private readonly Dictionary<string, ISFX> _SFXs;

		public SFXStorage(Dictionary<string, ISFX> SFXs) => _SFXs = SFXs;

        public IEnumerator Run(string name, ActorSFX user, ActorSkin target) => _SFXs[name].Run(user, target);
    }
}
