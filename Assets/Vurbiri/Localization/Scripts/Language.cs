using System;
using System.Collections.Generic;
using System.IO;
using Vurbiri.Reactive;
using static Vurbiri.Storage;

namespace Vurbiri.Localization
{
    public class Language : AReactive<Language>
    {
        private static readonly Language _instance;

        private readonly bool _isValid;
        private readonly string _folder;
        private readonly EnumArray<Files, bool> _loadFiles;
        private readonly string[] _nameFiles = Enum<Files>.Names;
        private readonly Dictionary<string, string>[] _text;
        private readonly LanguageType[] _languages;
        private readonly int _countFiles;
        private LanguageType _currentLanguage;

        public static Language Instance => _instance;
        public static bool IsValid => _instance != null && _instance._isValid;

        public IEnumerable<LanguageType> Languages => _languages;
        public int CurrentId => _currentLanguage == null ? -1 : _currentLanguage.Id;


        static Language() => _instance ??= new Language();

        public static Unsubscriber<Language> Subscribing(Action<Language> action, bool calling = true) => _instance?.Subscribe(action, calling);

        private Language()
        {
            _isValid = false;
            
            using SettingsScriptable settings = ProjectSettingsScriptable.GetCurrentSettings();
            if (settings == null) return;

            _folder = settings.Folder;
            _loadFiles = new(settings.LoadFiles);
            string path = Path.Combine(_folder, settings.LanguageFile);

            if (!LoadObjectFromResourceJson(path, out _languages))
                return;

            foreach (var language in _languages)
                if (!language.LoadSprite(_folder))
                    return;

            _countFiles = _nameFiles.Length;
            _text = new Dictionary<string, string>[_countFiles];
            _isValid = true;
        }

        public bool TryIdFromCode(string code, out int id)
        {
            id = -1;
            if (string.IsNullOrEmpty(code))
                return false;

            foreach (LanguageType language in Languages)
            {
                if (language.Code.ToLowerInvariant() == code.ToLowerInvariant())
                {
                    id = language.Id;
                    return true;
                }
            }
            return false;
        }

        public bool LoadFile(Files file)
        {
            int id = (int)file;
            return _loadFiles[id] || (_loadFiles[id] = LoadingFile(id, _currentLanguage));
        }

        public void UnloadFile(Files file)
        {
            int id = (int)file;
            _text[id] = null;
            _loadFiles[id] = false;
        }

        public bool SwitchLanguage(string code)
        {
            if (TryIdFromCode(code, out int id))
                return SwitchLanguage(id);

            return false;
        }

        public bool SwitchLanguage(int id)
        {
            if (_currentLanguage != null && _currentLanguage.Id == id) return true;

            foreach (LanguageType language in Languages)
                if (language.Id == id)
                    return SetLanguage(language);

            return false;
        }

        public string GetText(Files file, string key) => GetText(idFile: (int)file, key);
        public string GetText(Files file, object obj) => GetText(idFile: (int)file, obj.ToString());
        public string GetText(int idFile, string key)
        {
            if (_text[idFile] != null && _text[idFile].TryGetValue(key, out string str))
                return str;

            string msg = $"ERROR! File:[{(Files)idFile} : {_text[idFile] != null}] Key: [{key} : {_text[idFile]?.ContainsKey(key)}]";
            Message.Log(msg);
            return msg;
        }

        public string GetTextFormat(Files file, string key, params object[] args) => string.Format(GetText(idFile: (int)file, key), args);
        public string GetTextFormat(Files file, string key, object arg0, object arg1, object arg2) => string.Format(GetText(idFile: (int)file, key), arg0, arg1, arg2);
        public string GetTextFormat(Files file, string key, object arg0, object arg1) => string.Format(GetText(idFile: (int)file, key), arg0, arg1);
        public string GetTextFormat(Files file, string key, object arg0) => string.Format(GetText(idFile: (int)file, key), arg0);

        private bool SetLanguage(LanguageType type)
        {
            for (int i = 0; i < _countFiles; i++)
            {
                if (!_loadFiles[i])
                    continue;

                if (!LoadingFile(i, type))
                    return false;
            }

            _currentLanguage = type;
            ActionThisChange?.Invoke(this);
            return true;
        }

        private bool LoadingFile(int idFile, LanguageType type)
        {
            if (!LoadObjectFromResourceJson(Path.Combine(_folder, type.Folder, _nameFiles[idFile]), out Dictionary<string, string> load))
                return false;

            var current = _text[idFile];
            if (current == null)
            {
                _text[idFile] = new(load, new StringComparer());
                return true;
            }

            foreach (var item in load)
                current[item.Key] = item.Value;

            return true;
        }

        protected override void Callback(Action<Language> action) => action(this);

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
