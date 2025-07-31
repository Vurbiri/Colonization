using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    sealed public class RangeHitParticle : APooledSFX
    {
        private readonly AudioClip _clipRun, _clipHit;
        private readonly ParticleSystem _particle;
        private readonly WaitScaledTime _waitTime = new(0f);
        private readonly WaitSignal _waitActivate = new();
        private readonly float _avgSpeed;
        private ParticleSystem.MainModule _main;

        public RangeHitParticle(CreatorRangeHitParticle creator, Action<APooledSFX> deactivate) : base(creator, deactivate)
        {
            _clipRun = creator.clipRun; _clipHit = creator.clipHit;
            _particle = creator.particle;

            _main = _particle.main;
            _avgSpeed = (_main.startSpeed.constantMin + _main.startSpeed.constantMax) * 0.5f;
        }

        public override IEnumerator Hit(ISFXUser user, ActorSkin target)
        {
            Bounds bounds = target.Bounds;
            Vector3 targetPosition = target.Transform.position;
            targetPosition.y += bounds.extents.y * 1.1f;

            Enable(user.StartPosition);
            _transform.LookAt(targetPosition);

            float hitTime = (Vector3.Distance(_transform.position, targetPosition) - bounds.extents.z) / _avgSpeed;
            float particleTime = hitTime * 1.1f;
            _main.startLifetime = particleTime * 1.1f;
            _main.duration = particleTime * 1.1f;

            _particle.Play();
            user.AudioSource.PlayOneShot(_clipRun);

            _waitActivate.Reset();
            React_Cn(target, hitTime).Start();
            return _waitActivate;
        }

        private IEnumerator React_Cn(ActorSkin target, float time)
        {
            yield return _waitTime.Restart(time);
            _waitActivate.Send();
            target.Impact(_clipHit);

            yield return _waitTime.Restart(_clipHit.length);

            Disable();
        }
    }
}
