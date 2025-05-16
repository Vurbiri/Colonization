//Assets\Vurbiri.International\Runtime\Localization.cs
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine;
using Vurbiri.Reactive;
using static Vurbiri.Storage;

namespace Vurbiri.International
{
    public class Localization : IReactive<Localization>
    {
        private static readonly Localization s_instance;

        private readonly Dictionary<Files, string> _files;
        private readonly Dictionary<Files, Dictionary<string, string>> _text;
        private readonly ReadOnlyCollection<LanguageType> _languages;
        private readonly Signer<Localization> _changed = new();
        private readonly LanguageType _defaultLanguage;
        private LanguageType _currentLanguage;

        public static Localization Instance => s_instance;
        public ReadOnlyCollection<LanguageType> Languages => _languages;
        public SystemLanguage CurrentId => _currentLanguage.Id;
        public ICollection<Files> LoadedFiles => _text.Keys;

        static Localization() => s_instance = new();
        private Localization() 
        {
            _languages = new(LoadObjectFromResourceJson<LanguageType[]>(CONST_L.FILE_LANG));
            Throw.IfLengthZero(_languages);

            _defaultLanguage = _languages[0];
            for (int i = _languages.Count - 1; i >= 0; i--)
            {
                if (_languages[i].Equals(SystemLanguage.Unknown))
                {
                    _defaultLanguage = _languages[i];
                    break;
                }
            }

            Files[] values = Enum<Files>.Values;

            _files = new(Enum<Files>.count);
            _text = new(Enum<Files>.count);

            for (int i = 0; i < Enum<Files>.count; i++)
                _files[values[i]] = values[i].ToString();

            SetLanguage(_defaultLanguage);
            SetFiles(values[0]);
        }

#if UNITY_EDITOR
        public Localization(EnumFlags<Files> files) : base() => SetFiles(files);
#endif

        public Unsubscriber Subscribe(Action<Localization> action, bool sendCallback = true) => _changed.Add(action, sendCallback, this);

        public SystemLanguage IdFromCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                for (int i = _languages.Count - 1; i >= 0; i--)
                    if (_languages[i].Code.ToLowerInvariant() == code.ToLowerInvariant())
                        return _languages[i].Id;
            }

            return _defaultLanguage.Id;
        }

        public bool IsFileLoaded(Files file) => _text.ContainsKey(file);

        public void SetFiles(EnumFlags<Files> files)
        {
            Files file;
            for (int i = files.Count - 1; i >= 0; i--)
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

        public void SwitchLanguage(string code) => SwitchLanguage(IdFromCode(code));

        public void SwitchLanguage(SystemLanguage id)
        {
            if (_currentLanguage.Equals(id))
                return;

            for (int i = _languages.Count - 1; i >= 0; i--)
            {
                if (_languages[i].Equals(id))
                {
                    SetLanguage(_languages[i]);
                    return;
                }
            }

            SetLanguage(_defaultLanguage);
        }

        public string GetText(Files file, string key)
        {
            if (!_text.TryGetValue(file, out var dictionary))
            {
                Message.Log($"ERROR! File '{_files[file]}' not loaded.");
                return key;
            }
            if (!dictionary.TryGetValue(key, out string str))
            {
                Message.Log($"ERROR! Key '{key}' not found in file '{_files[file]}'.");
                return key;
            }

            return str;
        }

        public string GetText(string key)
        {
            foreach (var dictionary in _text.Values)
                if (dictionary.TryGetValue(key, out string str))
                    return str;

            Message.Log($"ERROR! Key '{key}' not found.");
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
            _changed.Invoke(this);
            return true;
        }

        protected bool LoadingFile(Files file, LanguageType type)
        {
            if (TryLoadObjectFromResourceJson(Path.Combine(type.Folder, _files[file]), out Dictionary<string, string> load))
            {
                if (_text.TryGetValue(file, out var current))
                {
                    foreach (var item in load)
                        current[item.Key] = item.Value;
                }
                else
                {
                    _text[file] = new(load, new StringComparer());
                }
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
