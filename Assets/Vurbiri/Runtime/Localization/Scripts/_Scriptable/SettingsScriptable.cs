//Assets\Vurbiri\Runtime\Localization\Scripts\_Scriptable\SettingsScriptable.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Localization
{
    using Collections;
    using static CONST_L;

    public class SettingsScriptable : ScriptableObject, IDisposable
    {
        [SerializeField] private string _folderPath = FOLDER_PATH;
        [SerializeField] private string _folder = FOLDER;
        [SerializeField] private string _filePath = FILE_PATH;
        [SerializeField] private string _languagesFile = FILE_LANG;
        [SerializeField] private EnumArray<Files, bool> _startLoadFiles = new(true);

        public string FolderPath => _folderPath;
        public string Folder => _folder;
        public string FilePath => _filePath;
        public string LanguageFile => _languagesFile;
        public IReadOnlyList<bool> LoadFiles => _startLoadFiles.Values;

        public void Dispose() => Resources.UnloadAsset(this);
    }
}
