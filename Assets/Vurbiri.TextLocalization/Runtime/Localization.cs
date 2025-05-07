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
        private static readonly string[] _nameFiles = Enum<Files>.Names;

        private readonly Dictionary<string, string>[] _text;
        private readonly LanguageType[] _languages;
        private readonly int _languagesCount;
        private readonly int _countFiles;
        private readonly Signer<Localization> _subscriber = new();
        private LanguageType _currentLanguage;

        public static Localization Instance => _instance;

        public IReadOnlyList<LanguageType> Languages => _languages;
        public int CurrentId => _currentLanguage.Id;

        static Localization() => _instance = new();
        private Localization()
        {
            if (!TryLoadObjectFromResourceJson(CONST_L.FILE_LANG, out _languages))
                Errors.Message("Localization. Error loading LanguageType");

            _languagesCount = _languages.Length;
            for (int i = 0; i < _languagesCount; i++)
                _languages[i].LoadSprite();

            _countFiles = _nameFiles.Length;
            _text = new Dictionary<string, string>[_countFiles];
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
            for(int i = 0; i < _languagesCount; i++)
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

        public void SetFiles(IReadOnlyList<bool> files)
        {
            int count = _countFiles <= files.Count ? _countFiles : files.Count;
            for (int i = 0; i < count; i++)
            {
                if (!files[i])
                {
                    _text[i] = null;
                    continue;
                }

                if (_text[i] == null)
                    LoadingFile(i, _currentLanguage);
            }

            GC.Collect();
        }

        public void SetFiles(EnumFlags<Files> files)
        {
            int count = _countFiles <= files.Count ? _countFiles : files.Count;
            for (int i = 0; i < count; i++)
            {
                if (!files[i])
                {
                    _text[i] = null;
                    continue;
                }

                if (_text[i] == null)
                    LoadingFile(i, _currentLanguage);
            }

            GC.Collect();
        }

        public bool LoadFile(Files file)
        {
            int id = (int)file;
            return _text[id] != null || LoadingFile(id, _currentLanguage);
        }

        public void UnloadFile(Files file)
        {
            _text[(int)file] = null;
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
            if (id >= _languagesCount)
                return false;

            if (_currentLanguage.Id == id) 
                return true;

            LanguageType language;
            for (int i = 0; i < _languagesCount; i++)
            {
                language = _languages[i];
                if (language.Id == id)
                    return SetLanguage(language);
            }

            return false;
        }

        public string GetText(Files file, string key) => GetText(idFile: (int)file, key);
        public string GetText(int idFile, string key)
        {
            if (_text[idFile] != null && _text[idFile].TryGetValue(key, out string str))
                return str;

            Message.Log($"ERROR! File:[{_nameFiles[idFile]} : {_text[idFile] != null}] Key: [{key} : {_text[idFile]?.ContainsKey(key)}]");
            return key;
        }

        public string GetTextFormat(Files file, string key, params object[] args) => string.Format(GetText(idFile: (int)file, key), args);
        public string GetTextFormat(Files file, string key, object arg0, object arg1, object arg2) => string.Format(GetText(idFile: (int)file, key), arg0, arg1, arg2);
        public string GetTextFormat(Files file, string key, object arg0, object arg1) => string.Format(GetText(idFile: (int)file, key), arg0, arg1);
        public string GetTextFormat(Files file, string key, object arg0) => string.Format(GetText(idFile: (int)file, key), arg0);

        private bool SetLanguage(LanguageType type)
        {
            for (int i = 0; i < _countFiles; i++)
            {
                if (_text[i] == null)
                    continue;

                if (!LoadingFile(i, type))
                    return false;
            }

            _currentLanguage = type;
            _subscriber.Invoke(this);
            return true;
        }

        private bool LoadingFile(int idFile, LanguageType type)
        {
            if (!TryLoadObjectFromResourceJson(Path.Combine(type.Folder, _nameFiles[idFile]), out Dictionary<string, string> load))
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
