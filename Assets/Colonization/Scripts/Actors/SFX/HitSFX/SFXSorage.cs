using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization
{
	public class SFXStorage
	{
		private readonly Dictionary<string, ISFX> _SFXs;

		public SFXStorage(Dictionary<string, ISFX> SFXs) => _SFXs = SFXs;

        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public IEnumerator Run(string name, ActorSFX user, ActorSkin target) => _SFXs[name].Run(user, target);
    }
}
