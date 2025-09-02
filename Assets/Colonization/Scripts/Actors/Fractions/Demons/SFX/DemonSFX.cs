using System.Collections;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
	public class DemonSFX : ActorSFX
    {
        protected string _specSFX;
        
        public override Vector3 StartPosition { get; } = Vector3.zero;

        public void Init(ReadOnlyArray<string> hitSFX, string specSFX)
        {
            InitInternal(hitSFX);
            _specSFX = specSFX;
        }

        public IEnumerator Spec(ActorSkin target) => GameContainer.HitSFX.Hit(_specSFX, this, target);
    }
}
