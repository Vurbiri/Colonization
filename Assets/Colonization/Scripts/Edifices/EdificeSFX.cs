using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
	public class EdificeSFX : MonoBehaviour
	{
        [SerializeField, Range(1f, 3f)] private float _height = 1.9f;
        [SerializeField, Range(1f, 10f)] private float _dropSpeed = 5f;
        [Space]
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private AudioSource _audioSource;

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
        private void OnValidate()
        {
            if (_particleSystem == null)
                _particleSystem = GetComponent<ParticleSystem>();
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();

            if(_audioSource.playOnAwake)
                _audioSource.playOnAwake = false;

            ParticleSystem.MainModule main = _particleSystem.main;
            if (main.playOnAwake)
                main.playOnAwake = false;
            if (main.stopAction != ParticleSystemStopAction.Destroy)
                main.stopAction = ParticleSystemStopAction.Destroy;
        }
#endif
    }
}
