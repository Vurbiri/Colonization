using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
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
        private readonly Subscription<Localization> _changed = new();
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
        public Localization(int fileId) : this() => LoadFile(fileId);
#endif

        public Unsubscription Subscribe(Action<Localization> action, bool sendCallback = true) => _changed.Add(action, sendCallback, this);

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
                    LoadFile(i);
                else
                    _text[i] = null;
            }

            GC.Collect();
        }

        public void LoadFile(int fileId)
        {
            if (_text[fileId] == null) 
                LoadingFile(fileId, _currentLanguage);
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
            string output;
            var dictionary = _text[fileId];
            if (dictionary == null)
            {
                Message.Log(output = $"File '{_files[fileId]}' not loaded.");
            }
            else if (!dictionary.TryGetValue(key, out output))
            {
                Message.Log(output = $"Key '{key}' not found in file '{_files[fileId]}'.");
            }

            return output;
        }

        public string GetText(string key)
        {
            string output;
            if (!string.IsNullOrEmpty(key))
            {
                for (int i = 0; i < _countFiles; i++)
                    if (_text[i] != null && _text[i].TryGetValue(key, out output))
                        return output;
            }
            Message.Log(output = $"Key '{key}' not found.");
            return output;
        }

        public string GetFormatText(int fileId, string key, params object[] args) => string.Format(GetText(fileId, key), args);
        public string GetFormatText(int fileId, string key, object arg0, object arg1, object arg2) => string.Format(GetText(fileId, key), arg0, arg1, arg2);
        public string GetFormatText(int fileId, string key, object arg0, object arg1) => string.Format(GetText(fileId, key), arg0, arg1);
        public string GetFormatText(int fileId, string key, object arg0) => string.Format(GetText(fileId, key), arg0);

        private void SetLanguage(LanguageType type)
        {
            for (int i = 0; i < _countFiles; i++)
                if (_text[i] != null)
                    LoadingFile(i, type);

            _currentLanguage = type;
            _changed.Invoke(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void LoadingFile(int fileId, LanguageType type)
        {
            if (TryLoadObjectFromResourceJson(string.Concat(type.Folder, "/", _files[fileId]), out Dictionary<string, string> load))
                _text[fileId] = load; //new(load, StringComparer.OrdinalIgnoreCase)
        }
    }
}
