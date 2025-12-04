using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Reactive;
using static Vurbiri.Storage;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.International
{
    public class Localization : IReactive<Localization>
    {
        private static readonly Localization s_instance;

        private readonly Dictionary<string, string>[] _text;
        private readonly ReadOnlyArray<LanguageType> _languages;
        private readonly VAction<Localization> _changed = new();
        private readonly LanguageType _defaultLanguage;
        private LanguageType _currentLanguage;

        public static Localization Instance { [Impl(256)] get => s_instance; }
        public ReadOnlyArray<LanguageType> Languages { [Impl(256)] get => _languages; }
        public SystemLanguage CurrentId
        {
            [Impl(256)] 
            get => _currentLanguage.Id;
            set
            {
                if (_currentLanguage != value)
                {
                    for (int i = _languages.Count - 1; i >= 0; --i)
                    {
                        if (_languages[i] == value)
                        {
                            SetLanguage(_languages[i]);
                            return;
                        }
                    }

                    if (_currentLanguage != _defaultLanguage)
                        SetLanguage(_defaultLanguage);
                }
            }
        }

        static Localization() => s_instance = new(0);
        private Localization(int fileId) 
        {
            var languages = LoadObjectFromJsonResource<List<LanguageType>>(CONST_L.LANG_FILE);
            Throw.IfLengthZero(languages, "_languages");

            _text = new Dictionary<string, string>[Files.Count];

            for (int i = languages.Count - 1; i >= 0; --i)
            {
                if (languages[i] == SystemLanguage.Unknown)
                {
                    _defaultLanguage = languages.Extract(i);
                    break;
                }
            }

            _defaultLanguage ??= languages[0];
            _languages = new(languages.ToArray());

            SetLanguage(_defaultLanguage);
            LoadFile(fileId, false);
        }

        [Impl(256)] public Subscription Subscribe(Action<Localization> action, bool sendCallback = true) => _changed.Add(action, sendCallback, this);
        [Impl(256)] public void Unsubscribe(Action<Localization> action) => _changed.Remove(action);

        public SystemLanguage IdFromCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                for (int i = _languages.Count - 1; i >= 0; --i)
                    if (_languages[i].CodeEquals(code))
                        return _languages[i].Id;
            }

            return _defaultLanguage.Id;
        }

        [Impl(256)] public bool IsFileLoaded(int fileId) => _text[fileId] != null;

        public void SetFiles(FileIds fileIds, bool reload)
        {
            for (int i = 0; i < Files.Count; i++)
            {
                if (fileIds[i]) 
                    LoadFile(i, reload);
                else 
                    _text[i] = null;
            }

            //GC.Collect();
        }

        [Impl(256)] public void LoadFile(int fileId, bool reload)
        {
            if ((reload || _text[fileId] == null) && Files.TryLoad(_currentLanguage.Folder, fileId, out Dictionary<string, string> load))
                _text[fileId] = load;
        }

        [Impl(256)] public void UnloadFile(int fileId)
        {
            _text[fileId] = null;
            //GC.Collect();
        }

        [Impl(256)] public string GetText(FileIdAndKey idAndKey, bool remove = false) => GetText(idAndKey.id, idAndKey.key, remove);
        public string GetText(int fileId, string key, bool remove = false)
        {
            string text;
            var dictionary = _text[fileId];
            if (dictionary == null)
                Log.Info(text = $"File [{Files.GetName(fileId)}] not loaded.");
            else if (!dictionary.TryGetValue(key, out text))
                Log.Info(text = $"Key [{key}] not found in file [{Files.GetName(fileId)}].");
            else if (remove)
                dictionary.Remove(key);

            return text;
        }

        [Impl(256)] public bool TryGetText(FileIdAndKey idAndKey, out string text)
        {
            var dictionary = _text[idAndKey.id]; text = null;
            return dictionary != null && dictionary.TryGetValue(idAndKey.key, out text);
        }
        [Impl(256)] public bool TryGetText(int fileId, string key, out string text)
        {
            var dictionary = _text[fileId]; text = null;
            return dictionary != null && dictionary.TryGetValue(key, out text);
        }

        [Impl(256)] public string GetFormatText(int fileId, string key, params object[] args) => string.Format(GetText(fileId, key), args);
        [Impl(256)] public string GetFormatText(int fileId, string key, object arg0, object arg1, object arg2) => string.Format(GetText(fileId, key), arg0, arg1, arg2);
        [Impl(256)] public string GetFormatText(int fileId, string key, object arg0, object arg1) => string.Format(GetText(fileId, key), arg0, arg1);
        [Impl(256)] public string GetFormatText(int fileId, string key, object arg0) => string.Format(GetText(fileId, key), arg0);

        [Impl(256)] public bool RemoveKey(FileIdAndKey idAndKey) => _text[idAndKey.id] != null && _text[idAndKey.id].Remove(idAndKey.key);
        [Impl(256)] public bool RemoveKey(int fileId, string key) => _text[fileId] != null && _text[fileId].Remove(key);

        private void SetLanguage(LanguageType type)
        {
            string folder = type.Folder;
            for (int i = 0; i < Files.Count; i++)
                if (_text[i] != null && Files.TryLoad(folder, i, out Dictionary<string, string> load))
                    _text[i] = load;

            _currentLanguage = type;
            _changed.Invoke(this);
        }

#if UNITY_EDITOR
        private static WeakReference<Localization> s_weakLocalization;
        public static Localization ForEditor(int fileId)
        {
            Localization localization;
            if (s_weakLocalization == null)
            {
                localization = new(fileId);
                s_weakLocalization = new(localization);
            }
            else if (!s_weakLocalization.TryGetTarget(out localization))
            {
                localization = new(fileId);
                s_weakLocalization.SetTarget(localization);
            }
            else
            {
                localization.LoadFile(fileId, true);
            }
            return localization;
        }
#endif
    }
}
