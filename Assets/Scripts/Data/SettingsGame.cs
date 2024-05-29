using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SettingsGame : ASingleton<SettingsGame>
{
    [Space]
    [SerializeField] private int _qualityDesktop = 1;
    [SerializeField] private Profile _profileDesktop = new();
    [Space]
    [SerializeField] private int _qualityMobile = 0;
    [SerializeField] private Profile _profileMobile = new();
    [Space]
    [SerializeField] private float _audioMinValue = 0.0f;
    [SerializeField] private float _audioMaxValue = 1.0f;
    //[SerializeField] private float _audioMinValue = 0.01f;
    //[SerializeField] private float _audioMaxValue = 1.5845f;

    private Profile _profileCurrent = null;

    public float MinValue => _audioMinValue;
    public float MaxValue => _audioMaxValue;

    public bool IsDesktop { get; private set; } = true;
    
    private YandexSDK _ysdk;
    private Localization _localization;
    private readonly Dictionary<AudioType, IVolume> _volumes = new(Enum<AudioType>.Count);

    protected override void Awake()
    {
        base.Awake();

        IsDesktop = !UtilityJS.IsMobile;

        _ysdk = YandexSDK.InstanceF;
        _localization = Localization.InstanceF;

        _volumes[AudioType.Music] = MusicSingleton.InstanceF;
        _volumes[AudioType.SFX] = SoundSingleton.InstanceF;
    }

    public void SetPlatform()
    {
        if (_ysdk.IsPlayer)
            IsDesktop = _ysdk.IsDesktop;
    }

    public bool Initialize(bool isLoad)
    {
        DefaultProfile();

        bool result = isLoad && Load();
        Apply();

        return result;
    }

    public void SetVolume(AudioType type, float volume) => _volumes[type].Volume = volume;

    public float GetVolume(AudioType type) => _profileCurrent.volumes[type.ToInt()];

    public void Save(Action<bool> callback = null)
    {
        _profileCurrent.idLang = _localization.CurrentId;
        foreach (var type in Enum<AudioType>.GetValues())
            _profileCurrent.volumes[type.ToInt()] = _volumes[type].Volume;

        StartCoroutine(Storage.Save_Coroutine(_profileCurrent.key, _profileCurrent, callback));
    }
    private bool Load()
    {
        Return<Profile> data = Storage.Load<Profile>(_profileCurrent.key);
        if (data.Result)
            _profileCurrent.Copy(data.Value);

        return data.Result;
    }

    public void Cancel()
    {
        if (!Load())
            DefaultProfile();

        Apply();
    }

    private void DefaultProfile()
    {
        QualitySettings.SetQualityLevel(IsDesktop ? _qualityDesktop : _qualityMobile);
        _profileCurrent = (IsDesktop ? _profileDesktop : _profileMobile).Clone();

        if (_ysdk.IsInitialize)
            if (_localization.TryIdFromCode(_ysdk.Lang, out int id))
                _profileCurrent.idLang = id;
    }

    private void Apply()
    {
        _localization.SwitchLanguage(_profileCurrent.idLang);
        foreach (var type in Enum<AudioType>.GetValues())
            _volumes[type].Volume = _profileCurrent.volumes[type.ToInt()];
    }

    #region Nested Classe
    [System.Serializable]
    private class Profile
    {
        [JsonIgnore]
        public string key = "std";
        [JsonProperty("ilg")]
        public int idLang = 1;
        [JsonProperty("vls")]
        public float[] volumes = { 0.6f, 0.6f };

        [JsonConstructor]
        public Profile(int idLang, float[] volumes)
        {
            this.idLang = idLang;
            this.volumes = (float[])volumes.Clone();
        }

        public Profile() { }

        public void Copy(Profile profile)
        {
            if (profile == null) return;

            idLang = profile.idLang;
            profile.volumes.CopyTo(volumes, 0);
        }

        public Profile Clone()
        {
            return new(idLang, volumes) { key = key };
        }

    }
    #endregion
}
