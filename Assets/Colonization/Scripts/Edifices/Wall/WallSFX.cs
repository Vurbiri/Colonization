using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
	public class WallSFX : MonoBehaviour
	{
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private AudioSource _audioSource;
        [Space]
        [SerializeField, Range(2f, 6f)] private float _showSpeed = 3.7f;

        private readonly WaitSignal _signal = new();

        public WaitSignal Run(Transform transform)
        {
            _particleSystem.Play();
            _audioSource.Play();

            transform.localScale = new(1f, 0f, 1f);
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
            float progress = 0f;
            Vector3 scale = new(1f, 0f, 1f);

            while (progress < 1f)
            {
                scale.y = progress += Time.deltaTime * _showSpeed;
                transform.localScale = scale;
                yield return null;
            }
            transform.localScale = new(1f, 1f, 1f);

            _signal.Send();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_particleSystem == null)
                _particleSystem = GetComponent<ParticleSystem>();
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();

            if (Application.isPlaying) return;

            if (_audioSource.playOnAwake)
                _audioSource.playOnAwake = false;
            if (_audioSource.loop)
                _audioSource.loop = false;

            ParticleSystem.MainModule main = _particleSystem.main;
            if (main.playOnAwake || main.loop)
                main.playOnAwake = main.loop = false;
            if (main.stopAction != ParticleSystemStopAction.Destroy)
                main.stopAction = ParticleSystemStopAction.Destroy;
        }
#endif
    }
}
