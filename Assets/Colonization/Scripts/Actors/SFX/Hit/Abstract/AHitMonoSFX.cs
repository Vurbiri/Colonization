using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public abstract class AHitMonoSFX : MonoBehaviour, IHitSFX
    {
        protected GameObject _thisGO;
        protected Transform _thisTransform, _parent;

        public abstract IHitSFX Init(IDataSFX parent);
        public abstract CustomYieldInstruction Hit(ActorSkin target);

        protected void Init()
        {
            _thisGO = gameObject;
            _thisGO.SetActive(false);

            _thisTransform = transform;
            _parent = _thisTransform.parent;
        }

    }
}
