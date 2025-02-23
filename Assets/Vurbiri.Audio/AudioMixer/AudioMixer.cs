//Assets\Vurbiri.Audio\AudioMixer\AudioMixer.cs
using UnityEngine;
using UnityEngine.Audio;
using Vurbiri.Collections;

namespace Vurbiri
{
    [System.Serializable]
    public class AudioMixer<T> where T : IdType<T> 
    {
        private const float RATE_TO = 40f, RATE_FROM = 1f / RATE_TO, RATE_TO_PLUS = 2.5f, RATE_FROM_PLUS = 1f / RATE_TO_PLUS;
        
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private IdArray<T, string> _nameParams;

        public const float MIN_VALUE = 0.01f, MAX_VALUE = 1.5845f;

        public bool Mute { get => AudioListener.pause; set => AudioListener.pause = value; }

        public float this[int index] 
        {
            get
            {
                _audioMixer.GetFloat(_nameParams[index], out float db);
                return ConvertFromDB(db);
            }
            set => _audioMixer.SetFloat(_nameParams[index], ConvertToDB(value));
        }
        public float this[Id<T> index]
        {
            get
            {
                _audioMixer.GetFloat(_nameParams[index], out float db);
                return ConvertFromDB(db);
            }
            set => _audioMixer.SetFloat(_nameParams[index], ConvertToDB(value));
        }

        public AudioMixer(AudioMixer<T> other)
        {
            _audioMixer = other._audioMixer;
            _nameParams = other._nameParams;
        }

        private float ConvertToDB(float volume)
        {
            volume = Mathf.Log10(Mathf.Clamp(volume, MIN_VALUE, MAX_VALUE)) * RATE_TO;
            if (volume > 0) 
                volume *= RATE_TO_PLUS;

            return volume;
        }

        private float ConvertFromDB(float dB)
        {
            if (dB > 0) 
                dB *= RATE_FROM_PLUS;

            return Mathf.Clamp(Mathf.Pow(10, dB * RATE_FROM), MIN_VALUE, MAX_VALUE);
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (_audioMixer == null)
                _audioMixer = VurbiriEditor.Utility.FindAnyAsset<AudioMixer>();

            _nameParams = new(IdType<T>.Names);
        }
#endif
    }
}
