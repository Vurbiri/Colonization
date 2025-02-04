//Assets\Colonization\Scripts\Actors\SFX\Hit\Abstract\AHitScriptableSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    //[CreateAssetMenu(fileName = "AScriptableSFX", menuName = "Vurbiri/Colonization/ActorSFX/AScriptableSFX", order = 51)]
    public abstract class AHitScriptableSFX : ScriptableObject
	{
        public abstract IHitSFX Create(IActorSFX parent);
    }
}
