//Assets\Colonization\Scripts\Actors\SFX\Hit\Scriptables\HitMonoScriptableSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    [CreateAssetMenu(fileName = "ScriptableHitMonoSFX", menuName = "Vurbiri/Colonization/ActorSFX/HitMonoScriptableSFX", order = 51)]
    public class HitMonoScriptableSFX : AHitScriptableSFX
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
