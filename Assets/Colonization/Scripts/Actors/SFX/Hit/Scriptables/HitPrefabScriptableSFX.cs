//Assets\Colonization\Scripts\Actors\SFX\Hit\Scriptables\HitPrefabScriptableSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    [CreateAssetMenu(fileName = "ScriptableHitPrefabSFX", menuName = "Vurbiri/Colonization/ActorSFX/HitPrefabScriptableSFX", order = 51)]
    public class HitPrefabScriptableSFX : AHitScriptableSFX
    {
        [SerializeField] private AHitMonoSFX _prefab;

        public override IHitSFX Create(IActorSFX parent)
        {
            if(_prefab == null)
                return new HitEmptySFX();

            return Instantiate(_prefab, parent.Container).Init(parent);
        }
    }
}
