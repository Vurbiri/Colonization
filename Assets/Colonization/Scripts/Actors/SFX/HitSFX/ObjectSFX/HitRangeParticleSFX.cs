using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Vurbiri.Colonization.Actors
{
    sealed public class HitRangeParticleSFX : AMonoSFX
    {
        [SerializeField] private AudioClip _clipRun;
        [SerializeField] private AudioClip _clipHit;

        private ParticleSystem _particle;
        private MainModule _main;
        private readonly WaitScaledTime _waitTime = new(0f);
        private readonly WaitSignal _waitActivate = new();
        private float _avgSpeed;

        public override IHitSFX Init(Action<AMonoSFX> deactivate)
        {
            _particle = GetComponent<ParticleSystem>();
            _main = _particle.main;

            _avgSpeed = (_main.startSpeed.constantMin + _main.startSpeed.constantMax) / 2f;

            return base.Init(deactivate); ;
        }

        public override IEnumerator Hit(IUserSFX user, ActorSkin target)
        {
            _thisTransform.SetParent(user.StartTransform, false);

            Bounds bounds = target.Bounds;
            Vector3 targetPosition = target.Transform.position;
            targetPosition.y += bounds.extents.y;

            float time = (Vector3.Distance(_thisTransform.position, targetPosition) - bounds.extents.z) / _avgSpeed;
            _main.startLifetime = time;
            _main.duration = time;
                        
            _thisTransform.LookAt(targetPosition);

            _particle.Play();
            user.AudioSource.PlayOneShot(_clipRun);

            _waitActivate.Reset();
            StartCoroutine(React_Cn(target, time));
            return _waitActivate;
        }

        private IEnumerator React_Cn(ActorSkin target, float time)
        {
            yield return _waitTime.Restart(time);
            _waitActivate.Send();
            target.Impact(_clipHit);

            yield return _waitTime.Restart(0.25f);

            a_deactivate(this);
        }

    }
}
