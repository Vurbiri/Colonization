using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public abstract class APooledSFX : IHitSFX 
    {
        private readonly Action<APooledSFX> a_deactivate;

        protected readonly Transform _transform;
        protected readonly GameObject _gameObject;

        public APooledSFX(Component creator, Action<APooledSFX> deactivate)
        {
            a_deactivate = deactivate;

            _transform  = creator.transform;
            _gameObject = creator.gameObject;

            Disable();
            UnityEngine.Object.Destroy(creator);
        }

        public abstract IEnumerator Hit(ActorSFX user, ActorSkin target);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Enable(Vector3 position)
        {
            _transform.localPosition = position;
            _gameObject.SetActive(true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Disable()
        {
            _gameObject.SetActive(false);
            a_deactivate(this);
        }
    }
}
