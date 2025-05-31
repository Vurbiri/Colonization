using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
	public class EdificeSFX : MonoBehaviour
	{
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private AudioSource _audioSource;
        [Space]
        [SerializeField, Range(1f, 3f)] private float _height = 1.9f;
        [SerializeField, Range(1f, 10f)] private float _dropSpeed = 7.5f;

        private readonly WaitSignal _signal = new();

        public WaitSignal Run(Transform transform)
        {
            transform.localPosition = new(0f, _height, 0f);
            StartCoroutine(Run_Cn(transform));
            return _signal;
        }

        public WaitSignal Destroy()
        {
            Destroy(gameObject);
            return null;
        }

        private IEnumerator Run_Cn(Transform transform)
        {
            float progress = 1f;
            Vector3 position = new(0f, _height, 0f);

            while (progress > 0f)
            {
                progress -= Time.deltaTime * _dropSpeed;
                position.y = _height * progress;
                transform.localPosition = position;
                yield return null;
            }
            transform.localPosition = new(0f, 0f, 0f);

            _particleSystem.Play();
            _audioSource.Play();
        }

        private void OnDestroy()
        {
            _signal.Send();
        }

#if UNITY_EDITOR

        [Header("┌──────────── Editor ─────────────────────")]
        [SerializeField, Range(4f, 15f)] private float _particlePerRadius = 7f;

        private void OnValidate()
        {
            if (_particleSystem == null)
                _particleSystem = GetComponent<ParticleSystem>();
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();

            if(Application.isPlaying) return;

            if(_audioSource.playOnAwake)
                _audioSource.playOnAwake = false;
            if (_audioSource.loop)
                _audioSource.loop = false;

            ParticleSystem.MainModule main = _particleSystem.main;
            if (main.playOnAwake)
                main.playOnAwake = false;
            if (main.stopAction != ParticleSystemStopAction.Destroy)
                main.stopAction = ParticleSystemStopAction.Destroy;

            if (transform.parent == null)
                return;
            var sphereCollider = GetComponentInParent<SphereCollider>();
            if (sphereCollider == null)
                return;

            float radius = sphereCollider.radius;
            var shape = _particleSystem.shape;
            shape.radius = radius;
            var emission = _particleSystem.emission;
            ParticleSystem.Burst burst;
            float maxCount = 0;
            for (int i = 0; i < emission.burstCount; i++)
            {
                burst = emission.GetBurst(i);
                burst.count = _particlePerRadius * radius * (1f - 0.5f * i);
                emission.SetBurst(i, burst);
                maxCount += burst.count.constant * burst.cycleCount;
            }
            main.maxParticles = Mathf.CeilToInt(maxCount);
        }
#endif
    }
}
