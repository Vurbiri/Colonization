//Assets\Colonization\Scripts\Actors\SFX\Abstract\AWarriorSFX.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public abstract class AWarriorSFX : AActorSFX
    {
        protected virtual IEnumerator Start()
        {
            Vector3 scale = new(0.1f, 0.1f, 0.1f), delta = Vector3.one;
            while(scale.x <= 1f )
            {
                _thisTransform.localScale = scale;
                yield return null;
                scale += Time.deltaTime * delta;
            }
            _thisTransform.localScale = Vector3.one;
        }
    }
}
