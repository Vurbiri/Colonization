using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Storage;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Storage
{
	using static SAVE_KEYS;

	public class ProjectStorage : System.IDisposable
	{
		private readonly IStorageService _storage;
		private readonly AudioMixer<MixerId>.Converter _mixerConverter = new();
		private readonly Profile.Converter _profileConverter = new();
		private Subscription _subscription;

		[Impl(256)] public ProjectStorage(IStorageService storage) => _storage = storage;

		[Impl(256)] public void SetAndBindAudioMixer(AudioMixer<MixerId> mixer)
		{
			_storage.TryPopulate<AudioMixer<MixerId>>(VOLUMES, new AudioMixer<MixerId>.Converter(mixer));
			_subscription += mixer.Subscribe(self => _storage.Set(VOLUMES, self, _mixerConverter), false);
		}
		[Impl(256)] public void SetAndBindProfile(Profile profile)
		{
			_storage.TryPopulate<Profile>(PROFILE, new Profile.Converter(profile));
			_subscription += profile.Subscribe(self => _storage.Set(PROFILE, self, _profileConverter), false);
		}

		[Impl(256)] public bool TryLoadPlayerColors(out Color32[] colors) => _storage.TryGet(COLORS, out colors);
		[Impl(256)] public void BindPlayerColors(Event<ReadOnlyArray<Color32>> onChange)
		{
			_subscription += onChange.Add(Set);

			// ----------- Local ----------------
			void Set(ReadOnlyArray<Color32> colors)
			{
				bool isRemove = colors == null || colors.Count != PlayerId.HumansCount;
				if (!isRemove)
				{
					isRemove = colors[0].a == 0;
					for (int i = 1; isRemove & i < PlayerId.HumansCount; ++i)
						isRemove &= colors[i].a == 0;
				}

				if (isRemove)
					_storage.Remove(COLORS);
				else
					_storage.Set(COLORS, colors);
			}
		}

		[Impl(256)] public bool TryLoadPlayerNames(out string[] names) => _storage.TryGet(NAMES, out names);
		[Impl(256)] public void BindPlayerNames(Event<ReadOnlyArray<string>> onChange)
		{
			_subscription += onChange.Add(Set);

			// ----------- Local ----------------
			void Set(ReadOnlyArray<string> names)
			{
				bool isRemove = names == null || names.Count != PlayerId.HumansCount;
				if (!isRemove)
				{
					isRemove = string.IsNullOrEmpty(names[0]);
					for (int i = 1; isRemove & i < PlayerId.HumansCount; ++i)
						isRemove &= string.IsNullOrEmpty(names[i]);
				}

				if(isRemove)
					_storage.Remove(NAMES); 
				else
					_storage.Set(NAMES, names);
			}
		}
		
		public GameSettings LoadGameSettings()
		{
			bool notLoad = !_storage.TryGet(GAME_SETTINGS, out GameSettings settings);
			if (notLoad) settings = new();
 
			_subscription += settings.Subscribe(BindGameSettings, notLoad);
			return settings;
		}
		private void BindGameSettings(GameSettings settings, bool isClear)
		{
			_storage.Set(GAME_SETTINGS, settings);

			if (isClear)
				_storage.Clear(NotClear);
		}

		[Impl(256)] public void Save() => _storage.Save();

		[Impl(256)] public void Dispose() => _subscription?.Dispose();
	}
}
