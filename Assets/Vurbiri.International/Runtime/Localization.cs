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
        public SystemLanguage CurrentId { [Impl(256)] get => _currentLanguage.Id; }

        static Localization() => s_instance = new(0);
        private Localization(int fileId) 
        {
            _languages = new(LoadObjectFromJsonResource<LanguageType[]>(CONST_L.LANG_FILE));
            Throw.IfLengthZero(_languages, "_languages");

            _text = new Dictionary<string, string>[Files.Count];

            _defaultLanguage = _languages[0];
            for (int i = _languages.Count - 1; i > 0; --i)
            {
                if (_languages[i] == SystemLanguage.Unknown)
                {
                    _defaultLanguage = _languages[i];
                    break;
                }
            }

            SetLanguage(_defaultLanguage);
            LoadFile(fileId);
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

        public void SetFiles(FileIds fileIds)
        {
            for (int i = 0; i < Files.Count; i++)
            {
                if (fileIds[i]) 
                    LoadFile(i);
                else 
                    _text[i] = null;
            }

            //GC.Collect();
        }

        [Impl(256)] public void LoadFile(int fileId)
        {
            if (_text[fileId] == null && Files.TryLoad(_currentLanguage.Folder, fileId, out Dictionary<string, string> load))
                _text[fileId] = load;
        }

        [Impl(256)] public void UnloadFile(int fileId)
        {
            _text[fileId] = null;
            //GC.Collect();
        }

        public void SwitchLanguage(SystemLanguage id)
        {
            if (_currentLanguage != id)
            {
                for (int i = _languages.Count - 1; i >= 0; --i)
                {
                    if (_languages[i] == id)
                    {
                        SetLanguage(_languages[i]);
                        return;
                    }
                }

                if (_currentLanguage != _defaultLanguage)
                    SetLanguage(_defaultLanguage);
            }
        }

        [Impl(256)] public string GetText(FileIdAndKey idAndKey) => GetText(idAndKey.id, idAndKey.key);
        public string GetText(int fileId, string key)
        {
            string output;
            var dictionary = _text[fileId];
            if (dictionary == null)
                MsgNotLoaded(fileId, out output);
            else if (!dictionary.TryGetValue(key, out output))
                MsgNotFound(fileId, key, out output);

            return output;
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

        [Impl(256)] public string ExtractText(FileIdAndKey idAndKey) => ExtractText(idAndKey.id, idAndKey.key);
        public string ExtractText(int fileId, string key)
        {
            string output;
            var dictionary = _text[fileId];
            if (dictionary == null)
                MsgNotLoaded(fileId, out output);
            else if (!dictionary.TryGetValue(key, out output))
                MsgNotFound(fileId, key, out output);
            else
                dictionary.Remove(key);

            return output;
        }

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

        [Impl(256)] private void MsgNotLoaded(int fileId, out string output) => Log.Info(output = $"File [{Files.GetName(fileId)}] not loaded.");
        [Impl(256)] private void MsgNotFound(int fileId, string key, out string output) => Log.Info(output = $"Key [{key}] not found in file [{Files.GetName(fileId)}].");

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
                localization.LoadFile(fileId);
            }
            return localization;
        }
#endif
    }
}
