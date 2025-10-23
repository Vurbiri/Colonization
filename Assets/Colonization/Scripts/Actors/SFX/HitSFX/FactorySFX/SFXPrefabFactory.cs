using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = FILE_NAME, menuName = MENU + "Prefab", order = ORDER)]
    sealed public class SFXPrefabFactory : ASFXFactory
    {
        [Space]
        [SerializeField] private AMonoCreatorSFX _prefabCreator;
        [SerializeField] private int _count = 1;

        public override ISFX Create() => new SFXPool(_prefabCreator, _count);

#if UNITY_EDITOR
        public override TargetForSFX_Ed Target_Ed => _prefabCreator != null ? _prefabCreator.Target_Ed : TargetForSFX_Ed.None;
#endif
    }
}
