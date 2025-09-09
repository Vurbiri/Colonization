using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    sealed public class RangeHitParticle : APooledSFX
    {
        private readonly AudioClip _clipRun, _clipHit;
        private readonly ParticleSystem _particle;
        private readonly WaitScaledTime _waitTime = new();
        private readonly WaitSignal _waitActivate = new();
        private readonly float _lifetimeRate, _lifetimeMinRate, _lifetimeMaxRate;
        private readonly float _avgSpeed;
        private readonly float _targetHeightRate;
        private ParticleSystem.MainModule _main;

        public RangeHitParticle(CreatorRangeHitParticle creator, Action<APooledSFX> deactivate) : base(creator, deactivate)
        {
            _clipRun = creator.clipRun; _clipHit = creator.clipHit;
            _particle = creator.particle;
            _lifetimeRate = creator.particleLifetimeRate;
            _targetHeightRate = creator.targetHeightRate;

            _main = _particle.main;
            _main.loop = false;

            _avgSpeed = _main.startSpeed.GetAvgSpeed();
            if (_avgSpeed < 0.01f) _avgSpeed = _particle.velocityOverLifetime.z.GetAvgSpeed();

            var temptMinMax = _main.startLifetime;
            float lifetimeMin = temptMinMax.constantMin, lifetimeMax = temptMinMax.constantMax;
            float avgLife = (lifetimeMin + lifetimeMax) * 0.5f;
            _lifetimeMinRate = lifetimeMin / avgLife; _lifetimeMaxRate = lifetimeMax / avgLife;

            UnityEngine.Object.Destroy(creator);
        }

        public override IEnumerator Hit(ActorSFX user, ActorSkin target)
        {
            Vector3 targetPosition = target.GetPosition(_targetHeightRate);

            Enable(user.Position);
            _transform.LookAt(targetPosition);

            float hitTime = (Vector3.Distance(_transform.position, targetPosition) - target.Extents.z) / _avgSpeed;
            float particleTime = hitTime * _lifetimeRate;
            _main.startLifetime = new(particleTime * _lifetimeMinRate, particleTime *= _lifetimeMaxRate);
            _main.duration = particleTime * 1.1f;

            _particle.Play();
            user.Play(_clipRun);

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
