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

        private readonly Dictionary<string, string>[] _text;
        private readonly ReadOnlyCollection<LanguageType> _languages;
        private readonly Subscription<Localization> _changed = new();
        private readonly LanguageType _defaultLanguage;
        private LanguageType _currentLanguage;

        public static Localization Instance => s_instance;
        public ReadOnlyCollection<LanguageType> Languages => _languages;
        public SystemLanguage CurrentId => _currentLanguage.Id;

        static Localization() => s_instance = new(0);
        private Localization(int fileId) 
        {
            _languages = new(LoadObjectFromResourceJson<LanguageType[]>(CONST_L.FILE_LANG));
            Throw.IfLengthZero(_languages, "_languages");

            _text = new Dictionary<string, string>[Files.Count];

            _defaultLanguage = _languages[0];
            for (int i = _languages.Count - 1; i > 0; i--)
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

        public Unsubscription Subscribe(Action<Localization> action, bool sendCallback = true) => _changed.Add(action, sendCallback, this);
        public void Unsubscribe(Action<Localization> action) => _changed.Remove(action);

        public SystemLanguage IdFromCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                for (int i = _languages.Count - 1; i >= 0; i--)
                    if (_languages[i].CodeEquals(code))
                        return _languages[i].Id;
            }

            return _defaultLanguage.Id;
        }

        public bool IsFileLoaded(int fileId) => _text[fileId] != null;

        public void SetFiles(FileIds fileIds)
        {
            for (int i = 0; i < Files.Count; i++)
            {
                if (fileIds[i])  LoadFile(i);
                else            _text[i] = null;
            }

            GC.Collect();
        }

        public void LoadFile(int fileId)
        {
            if (_text[fileId] == null && Files.Load(_currentLanguage.Folder, fileId, out Dictionary<string, string> load))
                _text[fileId] = load;
        }

        public void UnloadFile(int fileId)
        {
            _text[fileId] = null;
            GC.Collect();
        }

        public void SwitchLanguage(SystemLanguage id)
        {
            if (_currentLanguage == id)
                return;

            for (int i = _languages.Count - 1; i >= 0; i--)
            {
                if (_languages[i] == id)
                {
                    SetLanguage(_languages[i]);
                    return;
                }
            }

            SetLanguage(_defaultLanguage);
        }

        public string GetText(FileIdAndKey idAndKey) => GetText(idAndKey.id, idAndKey.key);

        public string GetText(int fileId, string key)
        {
            string output;
            var dictionary = _text[fileId];
            if (dictionary == null)
            {
                Log.Info(output = $"File '{Files.GetName(fileId)}' not loaded.");
            }
            else if (!dictionary.TryGetValue(key, out output))
            {
                Log.Info(output = $"Key '{key}' not found in file '{Files.GetName(fileId)}'.");
            }

            return output;
        }

        public string GetText(string key)
        {
            string output;
            if (!string.IsNullOrEmpty(key))
            {
                for (int i = 0; i < Files.Count; i++)
                    if (_text[i] != null && _text[i].TryGetValue(key, out output))
                        return output;
            }
            Log.Info(output = $"Key '{key}' not found.");
            return output;
        }

        public string GetFormatText(int fileId, string key, params object[] args) => string.Format(GetText(fileId, key), args);
        public string GetFormatText(int fileId, string key, object arg0, object arg1, object arg2) => string.Format(GetText(fileId, key), arg0, arg1, arg2);
        public string GetFormatText(int fileId, string key, object arg0, object arg1) => string.Format(GetText(fileId, key), arg0, arg1);
        public string GetFormatText(int fileId, string key, object arg0) => string.Format(GetText(fileId, key), arg0);

        private void SetLanguage(LanguageType type)
        {
            string folder = type.Folder;
            for (int i = 0; i < Files.Count; i++)
                if (_text[i] != null && Files.Load(folder, i, out Dictionary<string, string> load))
                    _text[i] = load;

            _currentLanguage = type;
            _changed.Invoke(this);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private void LoadingFile(int fileId, LanguageType type)
        //{
        //    if (Files.Load(type.Folder, fileId, out Dictionary<string, string> load))
        //        _text[fileId] = load; //new(load, StringComparer.OrdinalIgnoreCase)
        //}


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
