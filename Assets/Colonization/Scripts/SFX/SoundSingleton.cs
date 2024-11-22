//Assets\Colonization\Scripts\SFX\SoundSingleton.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class SoundSingleton : ASingleton<SoundSingleton>, IVolume
    {
        [Space]
        //[SerializeField] private AudioClip _clipNewLevel;
        //[SerializeField] private AudioClip _clipLevelComplete;
        //[SerializeField] private AudioClip _clipHint;
        //[SerializeField] private AudioClip _clipTurn;
        //[SerializeField] private AudioClip _clipLaser;
        //[SerializeField] private AudioClip _clipLaserOff;
        //[SerializeField] private AudioClip _clipLaserFalling;
        //[SerializeField] private AudioClip _clipCheat;
        //[SerializeField] private AudioClip _clipMenu;

        private AudioSource _thisAudio;

        public float Volume
        {
            get => _thisAudio.volume;
            set
            {
                _thisAudio.volume = value;
                _notPlay = value < 0.001f;
            }
        }

        private bool _notPlay = false;

        protected override void Awake()
        {
            base.Awake();
            _thisAudio = GetComponent<AudioSource>();
        }

        //public void PlayNewLevel() => Play(_clipNewLevel);
        //public void PlayLevelComplete() => Play(_clipLevelComplete);
        //public void PlayHint() => Play(_clipHint);
        //public void PlayTurn() => Play(_clipTurn);
        //public void PlayLaser() => Play(_clipLaser);
        //public void PlayLaserOff() => Play(_clipLaserOff);
        //public void PlayLaserFalling() => Play(_clipLaserFalling);
        //public void PlayCheat() => Play(_clipCheat);
        //public void PlayMenu() => Play(_clipMenu);

        private void Play(AudioClip clip)
        {
            if (_notPlay) return;

            _thisAudio.PlayOneShot(clip);
        }
    }
}
