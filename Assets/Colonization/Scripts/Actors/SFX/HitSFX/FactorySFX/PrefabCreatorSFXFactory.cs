using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [CreateAssetMenu(fileName = "ASFX_", menuName = "Vurbiri/Colonization/ActorSFX/PrefabCreator", order = 51)]
    sealed public class PrefabCreatorSFXFactory : ASFXFactory
    {
        [Space]
        [SerializeField] private AMonoCreatorSFX _prefabCreator;
        [SerializeField] private int _count = 1;

        public override IHitSFX Create() => new CreatorSFXPool(_prefabCreator, _count);
    }
}
