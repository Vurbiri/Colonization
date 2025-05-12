//Assets\Colonization\Scripts\Actors\SFX\Hit\HitRangeParticleSFX.cs
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Vurbiri.Colonization.Actors
{
    sealed public class HitRangeParticleSFX : AHitMonoSFX
    {
        [SerializeField] private AudioClip _clipRun;
        [SerializeField] private AudioClip _clipHit;

        private AudioSource _audioSource;
        private ParticleSystem _particle;
        private MainModule _main;
        private readonly WaitTime _waitTime = new(0f);
        private readonly WaitSignal _waitActivate = new();
        private float _avgSpeed;

        public override IHitSFX Init(IDataSFX parent)
        {
            Init();

            _audioSource = parent.AudioSource;

            _particle = GetComponent<ParticleSystem>();
            _main = _particle.main;

            _avgSpeed = (_main.startSpeed.constantMin + _main.startSpeed.constantMax) / 2f;

            return this;
        }

        public override CustomYieldInstruction Hit(ActorSkin target)
        {
            Bounds bounds = target.Bounds;
            Vector3 targetPosition = target.transform.position;
            targetPosition.y += bounds.extents.y;

            float time = (Vector3.Distance(_thisTransform.position, targetPosition) - bounds.extents.z) / _avgSpeed;
            _main.startLifetime = time;
            _main.duration = time;
                        
            _thisTransform.LookAt(targetPosition);
            _thisGO.SetActive(true);

            _particle.Play();
            _audioSource.PlayOneShot(_clipRun);

            _waitActivate.Reset();
            StartCoroutine(React_Cn(target, time));
            return _waitActivate;
        }

        private IEnumerator React_Cn(ActorSkin target, float time)
        {
            yield return _waitTime.Restart(time);
            _waitActivate.Send();
            target.React(_clipHit);

            yield return _waitTime.Restart(0.25f);
            _thisGO.SetActive(false);
        }

    }
}
