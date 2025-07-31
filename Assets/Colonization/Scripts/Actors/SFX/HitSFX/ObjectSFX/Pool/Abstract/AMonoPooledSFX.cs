using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public abstract class AMonoPooledSFX : MonoBehaviour, IHitSFX
    {
        protected Transform _thisTransform;
        protected GameObject _thisGameObject;
        private Action<AMonoPooledSFX> a_deactivate;

        public abstract IEnumerator Hit(ISFXUser user, ActorSkin target);

        public virtual AMonoPooledSFX Init(Action<AMonoPooledSFX>  deactivate)
        {
            _thisTransform  = transform;
            _thisGameObject = gameObject;
            a_deactivate = deactivate;

            Disable();
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Enable(Vector3 position)
        {
            _thisTransform.localPosition = position;
            _thisGameObject.SetActive(true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Disable()
        {
            _thisGameObject.SetActive(false);
            a_deactivate(this);
        }
    }
}
