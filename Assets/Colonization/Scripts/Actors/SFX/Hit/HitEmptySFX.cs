//Assets\Colonization\Scripts\Actors\SFX\Hit\HitEmptySFX.cs
using UnityEngine;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class HitEmptySFX : IHitSFX
    {
        public CustomYieldInstruction Hit(ActorSkin target) => null;
    }
}
