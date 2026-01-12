using System;
using UnityEngine;
using UnityEngine.Audio;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable]
	public partial class AudioMixer<T> where T : IdType<T> 
	{
		private const float MIN_DB = -80, MAX_DB = 20f;
		public const float MIN_VALUE = 0.01f, MAX_VALUE = 1.6f;

		private static readonly float RATE_TO = MIN_DB / MathF.Log10(MIN_VALUE);
		private static readonly float RATE_TO_PLUS = MAX_DB / (MathF.Log10(MAX_VALUE) * RATE_TO);
		private static readonly float RATE_FROM = 1f / RATE_TO;
		private static readonly float RATE_FROM_PLUS = 1f / RATE_TO_PLUS;

		[SerializeField] private AudioMixer _audioMixer;
		[SerializeField] private IdArray<T, float> _volumes = new(0.8f);
		[SerializeField] private IdArray<T, string> _nameParams;

		private readonly VAction<AudioMixer<T>> _changeEvent = new();

		public float this[int index] 
		{
			[Impl(256)] get => _volumes[index];
			[Impl(256)] set => _audioMixer.SetFloat(_nameParams[index], ConvertToDB(value));
		}
		public float this[Id<T> index]
		{
			[Impl(256)] get => _volumes[index];
			[Impl(256)] set => _audioMixer.SetFloat(_nameParams[index], ConvertToDB(value));
		}

		public ReadOnlyIdArray<T, string> Names { [Impl(256)] get => _nameParams; }

		public void Apply()
		{
			bool changed = false;
			for (int i = 0; i < IdType<T>.Count; i++)
			{
				if (_audioMixer.GetFloat(_nameParams[i], out float value))
				{
					value = ConvertFromDB(value);
					changed |= _volumes[i] != value;
					_volumes[i] = value;
				}
			}

			if (changed) _changeEvent.Invoke(this);
		}

		public void Cancel()
		{
			for (int i = 0; i < IdType<T>.Count; i++)
				this[i] = _volumes[i];
		}

		[Impl(256)] public Subscription Subscribe(Action<AudioMixer<T>> action, bool instantGetValue = true) => _changeEvent.Add(action, this, instantGetValue);

		private float ConvertToDB(float volume)
		{
			volume = MathF.Log10(Mathf.Clamp(volume, MIN_VALUE, MAX_VALUE)) * RATE_TO;
			if (volume > 0)
				volume *= RATE_TO_PLUS;

			return volume;
		}

		private float ConvertFromDB(float dB)
		{
			if (dB > 0)
				dB *= RATE_FROM_PLUS;

			return Math.Clamp(MathF.Pow(10, dB * RATE_FROM), MIN_VALUE, MAX_VALUE);
		}

#if UNITY_EDITOR
		public void OnValidate()
		{
			EUtility.SetAsset(ref _audioMixer);

			if (_nameParams == null)
			{
				_nameParams = new(IdType<T>.Names_Ed);
			}
			else
			{
				for (int i = 0; i < IdType<T>.Count; i++)
					if (string.IsNullOrEmpty(_nameParams[i]))
						_nameParams[i] = IdType<T>.Names_Ed[i];
			}
		}
#endif
	}
}
