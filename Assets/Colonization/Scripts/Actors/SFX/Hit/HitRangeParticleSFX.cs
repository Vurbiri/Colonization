//Assets\Colonization\Scripts\Actors\SFX\Hit\HitRangeParticleSFX.cs
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Vurbiri.Colonization.Actors
{
    public class HitRangeParticleSFX : AHitMonoSFX
    {
        [SerializeField] private AudioClip _clipRun;
        [SerializeField] private AudioClip _clipHit;

        private AudioSource _audioSource;
        private ParticleSystem _particle;
        private MainModule _main;
        private readonly WaitTime _waitTime = new(0);
        private readonly WaitActivate _waitActivate = new();

        public override IHitSFX Init(IDataSFX parent)
        {
            Init();

            _audioSource = parent.AudioSource;

            _particle = GetComponent<ParticleSystem>();
            _main = _particle.main;

            return this;
        }

        public override CustomYieldInstruction Hit(ActorSkin target)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.y += target.Bounds.extents.y;

            float time = (Vector3.Distance(_thisTransform.position, targetPosition) - target.Bounds.extents.z) / _main.startSpeed.constantMin;
            _main.startLifetime = time;
            _main.duration = time;
                        
            _thisTransform.LookAt(targetPosition);
            _thisGO.SetActive(true);

            _particle.Play();
            _audioSource.PlayOneShot(_clipRun);

            _waitActivate.Reset();
            StartCoroutine(React_Coroutine(target, time));
            return _waitActivate;
        }

        private IEnumerator React_Coroutine(ActorSkin target, float time)
        {
            yield return _waitTime.SetTime(time);
            _waitActivate.Activate();
            target.React(_clipHit);
            _thisGO.SetActive(false);
        }

    }
}
