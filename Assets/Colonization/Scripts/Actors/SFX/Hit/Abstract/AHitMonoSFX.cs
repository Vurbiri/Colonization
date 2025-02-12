//Assets\Colonization\Scripts\Actors\SFX\Hit\Abstract\AHitMonoSFX.cs
using UnityEngine;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AHitMonoSFX : MonoBehaviour, IHitSFX
    {
        protected GameObject _thisGO;
        protected Transform _thisTransform, _parent;

        protected virtual void Awake()
        {
            _thisGO = gameObject;
            _thisGO.SetActive(false);

            _thisTransform = transform;
            _parent = _thisTransform.parent;
        }

        public abstract CustomYieldInstruction Hit(ActorSkin target);

        public abstract IHitSFX Init(IActorSFX parent);
    }
}
