//Assets\Vurbiri.TextLocalization\Runtime\Localization.cs
using System;
using System.Collections.Generic;
using System.IO;
using Vurbiri.Reactive;
using static Vurbiri.Storage;

namespace Vurbiri.TextLocalization
{
    public class Localization : IReactive<Localization>
    {
        private static readonly Localization _instance;
        private readonly string[] _nameFiles = Enum<Files>.Names;

        private readonly Dictionary<Files, string> _files;
        private readonly Dictionary<Files, Dictionary<string, string>> _text;
        private readonly LanguageType[] _languages;
        private readonly Signer<Localization> _subscriber = new();
        private LanguageType _currentLanguage;

        public static Localization Instance => _instance;

        public IReadOnlyList<LanguageType> Languages => _languages;
        public int CurrentId => _currentLanguage.Id;

        static Localization() => _instance = new();
        private Localization()
        {
            _languages = LoadObjectFromResourceJson<LanguageType[]>(CONST_L.FILE_LANG);

            for (int i = 0; i < _languages.Length; i++)
                _languages[i].LoadSprite();

            var values = Enum<Files>.Values;
            
            _files = new(Enum<Files>.count);
            _text = new(Enum<Files>.count);

            for(int i = 0; i < Enum<Files>.count; i++)
                _files[values[i]] = values[i].ToString();
            
            SetLanguage(_languages[0]);
        }

#if UNITY_EDITOR
        public Localization(EnumFlags<Files> files) : this() => SetFiles(files);
#endif

        public Unsubscriber Subscribe(Action<Localization> action, bool sendCallback = true) => _subscriber.Add(action, sendCallback, this);

        public bool TryIdFromCode(string code, out int id)
        {
            id = -1;
            if (string.IsNullOrEmpty(code))
                return false;

            LanguageType language;
            for (int i = 0; i < _languages.Length; i++)
            {
                language = _languages[i];
                if (language.Code.ToLowerInvariant() == code.ToLowerInvariant())
                {
                    id = language.Id;
                    return true;
                }
            }
            return false;
        }

        public void SetFiles(EnumFlags<Files> files)
        {
            Files file;
            for (int i = 0; i < files.Count; i++)
            {
                file = (Files)i;
                if (files[i])
                {
                    if (!_text.ContainsKey(file))
                        LoadingFile(file, _currentLanguage);
                }
                else 
                {
                    _text.Remove(file);
                }
            }

            GC.Collect();
        }

        public bool LoadFile(Files file)
        {
            return _text.ContainsKey(file) || LoadingFile(file, _currentLanguage);
        }

        public void UnloadFile(Files file)
        {
            _text.Remove(file);
            GC.Collect();
        }

        public bool SwitchLanguage(string code)
        {
            if (TryIdFromCode(code, out int id))
                return SwitchLanguage(id);

            return false;
        }

        public bool SwitchLanguage(int id)
        {
            if (_currentLanguage.Id == id) 
                return true;

            LanguageType language;
            for (int i = 0; i < _languages.Length; i++)
            {
                language = _languages[i];
                if (language.Id == id)
                    return SetLanguage(language);
            }

            return false;
        }

        public string GetText(Files file, string key)
        {
            if (_text.TryGetValue(file, out var dictionary) && dictionary.TryGetValue(key, out string str))
                return str;

            Message.Log($"ERROR! File:[{_files[file]} : {_text.ContainsKey(file)} Key: [{key} : {dictionary?.ContainsKey(key)}]");
            return key;
        }

        public string GetTextFormat(Files file, string key, params object[] args) => string.Format(GetText(file, key), args);
        public string GetTextFormat(Files file, string key, object arg0, object arg1, object arg2) => string.Format(GetText(file, key), arg0, arg1, arg2);
        public string GetTextFormat(Files file, string key, object arg0, object arg1) => string.Format(GetText(file, key), arg0, arg1);
        public string GetTextFormat(Files file, string key, object arg0) => string.Format(GetText(file, key), arg0);

        private bool SetLanguage(LanguageType type)
        {
           foreach (var key in _text.Keys)
                if (!LoadingFile(key, type))
                    return false;

            _currentLanguage = type;
            _subscriber.Invoke(this);
            return true;
        }

        private bool LoadingFile(Files file, LanguageType type)
        {
            if (TryLoadObjectFromResourceJson(Path.Combine(type.Folder, _files[file]), out Dictionary<string, string> load))
            { 
                if (_text.TryGetValue(file, out var current))
                {
                    foreach (var item in load)
                        current[item.Key] = item.Value;
                    return true;
                }

                _text[file] = new(load, new StringComparer());
                return true;
            }

            return false;
        }

        #region Nested: StringComparer
        //***********************************************************
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
}
