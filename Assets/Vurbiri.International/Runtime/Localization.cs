//Assets\Vurbiri.International\Runtime\Localization.cs
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Vurbiri.Reactive;
using static Vurbiri.Storage;

namespace Vurbiri.International
{
    public class Localization : IReactive<Localization>
    {
        private static readonly Localization s_instance;

        private readonly int _countFiles;
        private readonly string[] _files;
        private readonly Dictionary<string, string>[] _text;
        private readonly ReadOnlyCollection<LanguageType> _languages;
        private readonly Signer<Localization> _changed = new();
        private readonly LanguageType _defaultLanguage;
        private LanguageType _currentLanguage;

        public static Localization Instance => s_instance;
        public ReadOnlyCollection<LanguageType> Languages => _languages;
        public SystemLanguage CurrentId => _currentLanguage.Id;
        public int CountFiles => _countFiles;

        static Localization() => s_instance = new();
        private Localization() 
        {
            _files = LoadObjectFromResourceJson<string[]>(CONST_L.FILE_FILES);
            Throw.IfLengthZero(_files);
            _languages = new(LoadObjectFromResourceJson<LanguageType[]>(CONST_L.FILE_LANG));
            Throw.IfLengthZero(_languages);

            _countFiles = _files.Length;
            _text = new Dictionary<string, string>[_countFiles];

            _defaultLanguage = _languages[0];
            for (int i = _languages.Count - 1; i >= 0; i--)
            {
                if (_languages[i].Equals(SystemLanguage.Unknown))
                {
                    _defaultLanguage = _languages[i];
                    break;
                }
            }

            SetLanguage(_defaultLanguage);
            LoadFile(0);
        }

#if UNITY_EDITOR
        public Localization(int fileId) : base() => LoadFile(fileId);
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

        public bool IsFileLoaded(int fileId) => _text[fileId] != null;

        public void SetFiles(FileIds fileIds)
        {
            for (int i = 0; i < _countFiles; i++)
            {
                if (fileIds[i])
                {
                    if (_text[i] == null)
                        LoadingFile(i, _currentLanguage);
                }
                else
                {
                    _text[i] = null;
                }
            }

            GC.Collect();
        }

        public bool LoadFile(int fileId)
        {
            return _text[fileId] != null || LoadingFile(fileId, _currentLanguage);
        }

        public void UnloadFile(int fileId)
        {
            _text[fileId] = null;
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

        public string GetText(int fileId, string key)
        {
            var dictionary = _text[fileId];
            if (dictionary == null)
            {
                Message.Log($"ERROR! File '{_files[fileId]}' not loaded.");
                return key;
            }
            if (!dictionary.TryGetValue(key, out string str))
            {
                Message.Log($"ERROR! Key '{key}' not found in file '{_files[fileId]}'.");
                return key;
            }

            return str;
        }

        public string GetText(string key)
        {
            for (int i = 0; i < _countFiles; i++)
            {
                if (_text[i] != null && _text[i].TryGetValue(key, out string str))
                    return str;
            }

            Message.Log($"ERROR! Key '{key}' not found.");
            return key;
        }

        public string GetTextFormat(int fileId, string key, params object[] args) => string.Format(GetText(fileId, key), args);
        public string GetTextFormat(int fileId, string key, object arg0, object arg1, object arg2) => string.Format(GetText(fileId, key), arg0, arg1, arg2);
        public string GetTextFormat(int fileId, string key, object arg0, object arg1) => string.Format(GetText(fileId, key), arg0, arg1);
        public string GetTextFormat(int fileId, string key, object arg0) => string.Format(GetText(fileId, key), arg0);

        private bool SetLanguage(LanguageType type)
        {
            for (int i = 0; i < _countFiles; i++)
            {
                if(_text[i] != null)
                    if (!LoadingFile(i, type))
                        return false;
            }

            _currentLanguage = type;
            _changed.Invoke(this);
            return true;
        }

        protected bool LoadingFile(int fileId, LanguageType type)
        {
            if (TryLoadObjectFromResourceJson(string.Concat(type.Folder, "/", _files[fileId]), out Dictionary<string, string> load))
            {
                var current = _text[fileId];
                if (current != null)
                {
                    foreach (var item in load)
                        current[item.Key] = item.Value;
                }
                else
                {
                    _text[fileId] = new(load, new StringComparer());
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
