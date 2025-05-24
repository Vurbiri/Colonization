using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public interface IHitSFX
	{
        public CustomYieldInstruction Hit(ActorSkin target);
    }
}
