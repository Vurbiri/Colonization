using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public partial class Localization : ASingleton<Localization>
{
    [SerializeField] private string _path = "Languages";
    [SerializeField] private string _defaultLang = "ru";

    private Dictionary<string, string> _language = new();

    public LanguageType[] Languages { get; private set; }
    public int CurrentId { get; private set; } = -1;

    public event Action<Localization> EventSwitchLanguage;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    public bool Initialize()
    {
        Return<LanguageType[]> lt = StorageResources.LoadFromJson<LanguageType[]>(_path);
        if (lt.Result)
        {
            Languages = lt.Value;
            return SwitchLanguage(_defaultLang);
        }

        return false;
    }

    public bool TryIdFromCode(string codeISO639_1, out int id)
    {
        id = -1;
        if (string.IsNullOrEmpty(codeISO639_1)) 
            return false;

        foreach (LanguageType language in Languages)
        {
            if (language.CodeISO639_1.ToLowerInvariant() == codeISO639_1.ToLowerInvariant())
            {
                id = language.Id;
                return true;
            }
        }
        return false;  
    }

    public bool SwitchLanguage(string codeISO639_1)
    {
        if (TryIdFromCode(codeISO639_1, out int id))
            return SwitchLanguage(id);

        return false;
    }

    public bool SwitchLanguage(int id)
    {
        if (CurrentId == id) return true;

        foreach (LanguageType language in Languages)
            if (language.Id == id)
                return SetLanguage(language);

        return false;
    }

    public string GetText(string key)
    {
        if (_language.TryGetValue(key, out string str))
            return str;

        return "ERROR! " + key;
    }

    //public string GetTextFormat(string key, params object[] args) => string.Format(GetText(key), args);
    //public string GetTextFormat(string key, object arg0, object arg1, object arg2) => string.Format(GetText(key), arg0, arg1, arg2);
    //public string GetTextFormat(string key, object arg0, object arg1) => string.Format(GetText(key), arg0, arg1);
    public string GetTextFormat(string key, object arg0) => string.Format(GetText(key), arg0);

    private bool SetLanguage(LanguageType type)
    {
        Return<Dictionary<string, string>> d = StorageResources.LoadFromJson<Dictionary<string, string>>(type.File);
        if (d.Result)
        {
            CurrentId = type.Id;
            _language = new(d.Value, new StringComparer());
            EventSwitchLanguage?.Invoke(this);
        }

        return d.Result;
    }

    #region Nested ñlasses: LanguageType, StringComparer
    public class LanguageType
    {
        public int Id { get; private set; }
        public string CodeISO639_1 { get; private set; }
        public string Name { get; private set; }
        public string File { get; private set; }
        [JsonIgnore]
        public Sprite Sprite { get; private set; }
        [JsonIgnore]
        private const string _pathBanner = "Banners/";

        [JsonConstructor]
        public LanguageType(int id, string codeISO639_1, string name, string file)
        {
            Id = id;
            CodeISO639_1 = codeISO639_1;
            Name = name;
            File = file;
            Sprite = Resources.Load<Sprite>(_pathBanner + File);
        }
    }

    public class StringComparer : IEqualityComparer<string>
    {
        public bool Equals(string str1, string str2)
        {
            return str1.ToLowerInvariant() == str2.ToLowerInvariant();
        }
        public int GetHashCode(string str)
        {
            return str.ToLowerInvariant().GetHashCode();
        }

    }
    #endregion
}
