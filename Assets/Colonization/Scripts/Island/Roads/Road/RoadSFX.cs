using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
	public class RoadSFX : MonoBehaviour
	{
        [SerializeField] private AudioClip _buildClip;
        [SerializeField] private AudioClip _removeClip;

        private Transform _thisTransform;
        private ParticleSystem _particleSystem;
        private AudioSource _audioSource;
        private WaitRealtime _whatTime;

        private void Awake()
		{
            _thisTransform = transform;
            _particleSystem = GetComponent<ParticleSystem>();
            _audioSource = GetComponent<AudioSource>();
            _whatTime = new(Mathf.Max(_particleSystem.main.duration, _removeClip.length));

            var shape = _particleSystem.shape;
            shape.radius = CONST.HEX_RADIUS_OUT * 0.5f;
        }

        public void Build(CrossroadLink link)
        {
            _thisTransform.localPosition = link.Position;
            _audioSource.PlayOneShot(_buildClip);
        }

        public IEnumerator Remove_Cn(CrossroadLink link)
        {
            _thisTransform.SetLocalPositionAndRotation(link.Position, CROSS.LINK_ROTATIONS[link.Id.Value]);
            _particleSystem.Play(); _audioSource.PlayOneShot(_removeClip);

            yield return _whatTime.Restart();
        }
	}
}
