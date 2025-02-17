//Assets\Colonization\Scripts\Actors\SFX\Hit\Scriptables\HitPrefabScriptableSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    [CreateAssetMenu(fileName = "ASFX_", menuName = "Vurbiri/Colonization/ActorSFX/HitPrefab", order = 51)]
    public class HitPrefabScriptableSFX : AHitScriptableSFX
    {
        [SerializeField] private AHitMonoSFX _prefab;

        public override IHitSFX Create(IActorSFX parent)
        {
            if(_prefab == null)
                return new HitEmptySFX();

            return Instantiate(_prefab, parent.Main).Init(parent);
        }
    }
}
