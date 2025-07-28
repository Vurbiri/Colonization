using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [CreateAssetMenu(fileName = "ASFX_", menuName = "Vurbiri/Colonization/ActorSFX/HitHandPrefab", order = 51)]
    public class HitHandPrefabScriptableSFX : APrefabSFXFactory
    {
        [SerializeField] private AMonoSFX _prefab;

        public override IHitSFX Create()
        {
            if (_prefab == null)
                return new EmptySFX();

            return Instantiate(_prefab, GameContainer.Repository).Init();
        }
    }
}
