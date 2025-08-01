using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [CreateAssetMenu(fileName = "ASFX_", menuName = "Vurbiri/Colonization/ActorSFX/Prefab", order = 51)]
    sealed public class PrefabSFXFactory : ASFXFactory
    {
        [Space]
        [SerializeField] private AMonoPooledSFX _prefab;
        [SerializeField] private int _count = 1;

        public override IHitSFX Create() => new MonoSFXPool(_prefab, _count);
    }
}
