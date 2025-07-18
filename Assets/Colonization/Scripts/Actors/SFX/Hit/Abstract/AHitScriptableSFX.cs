using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    //[CreateAssetMenu(fileName = "AScriptableSFX", menuName = "Vurbiri/Colonization/ActorSFX/AScriptableSFX", order = 51)]
    public abstract class AHitScriptableSFX : ScriptableObject
	{
        public abstract IHitSFX Create(IDataSFX parent);
    }
}
