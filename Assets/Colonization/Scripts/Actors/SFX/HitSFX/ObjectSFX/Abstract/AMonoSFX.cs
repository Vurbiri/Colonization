using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public abstract class AMonoSFX : MonoBehaviour, IHitSFX
    {
        protected Transform _thisTransform, _parent;
        protected Action<AMonoSFX> a_deactivate;

        public abstract IEnumerator Hit(IUserSFX user, ActorSkin target);

        public virtual IHitSFX Init()
        {
            _thisTransform = transform;
            _parent = _thisTransform.parent;

            return this;
        }

        public virtual IHitSFX Init(Action<AMonoSFX>  deactivate)
        {
            _thisTransform = transform;
            _parent = _thisTransform.parent;
            a_deactivate = deactivate;

            return this;
        }

        public void SetParent(Transform parent) => _thisTransform.SetParent(parent, false);

    }
}
