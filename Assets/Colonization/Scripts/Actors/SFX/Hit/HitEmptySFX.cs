//Assets\Colonization\Scripts\Actors\SFX\Hit\HitEmptySFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class HitEmptySFX : IHitSFX
    {
        public CustomYieldInstruction Hit(ActorSkin target) => null;
    }
}
