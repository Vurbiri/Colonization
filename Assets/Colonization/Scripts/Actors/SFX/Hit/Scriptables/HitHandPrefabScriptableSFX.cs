using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [CreateAssetMenu(fileName = "ASFX_", menuName = "Vurbiri/Colonization/ActorSFX/HitHandPrefab", order = 51)]
    public class HitHandPrefabScriptableSFX : AHitScriptableSFX
    {
        [SerializeField] private AHitMonoSFX _prefab;

        public override IHitSFX Create(IDataSFX parent)
        {
            if (_prefab == null)
                return new HitEmptySFX();

            return Instantiate(_prefab, parent.RightHand).Init(parent);
        }
    }
}
