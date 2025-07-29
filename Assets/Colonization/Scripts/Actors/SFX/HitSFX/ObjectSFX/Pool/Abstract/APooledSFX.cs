using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public abstract class APooledSFX : IHitSFX 
    {
        protected readonly Transform _transform;
        protected readonly GameObject _gameObject;
        private readonly Action<APooledSFX> a_deactivate;

        public APooledSFX(MonoBehaviour parent, Action<APooledSFX> deactivate)
        {
            _transform  = parent.transform;
            _gameObject = parent.gameObject;
            a_deactivate = deactivate;

            Disable();
        }

        public abstract IEnumerator Hit(IUserSFX user, ActorSkin target);


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
