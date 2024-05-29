using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicSingleton : ASingleton<MusicSingleton>, IVolume
{
    private AudioSource _thisAudio;

    public float Volume
    {
        get => _thisAudio.volume;
        set
        {
            _thisAudio.volume = value;
            if (value < 0.001f)
                _thisAudio.Pause();
            else if (!_thisAudio.isPlaying)
                _thisAudio.UnPause();
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _thisAudio = GetComponent<AudioSource>();
    }

    public void Play() => _thisAudio.Play();

}
