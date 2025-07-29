using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public abstract class AMonoSFX : MonoBehaviour, IHitSFX
    {
        protected Transform _thisTransform;
        protected Action<AMonoSFX> a_deactivate;

        public abstract IEnumerator Hit(IUserSFX user, ActorSkin target);

        public virtual IHitSFX Init(Action<AMonoSFX>  deactivate)
        {
            _thisTransform = transform;
            a_deactivate = deactivate;

            return this;
        }

        public void SetParent(Transform parent) => _thisTransform.SetParent(parent, false);

    }
}
