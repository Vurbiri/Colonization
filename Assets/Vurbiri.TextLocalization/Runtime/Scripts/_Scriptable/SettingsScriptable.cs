//Assets\Vurbiri.TextLocalization\Runtime\Scripts\_Scriptable\SettingsScriptable.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.TextLocalization
{
    using static CONST_L;

    public class SettingsScriptable : ScriptableObject, IDisposable
    {
        [SerializeField] private string _folderPath = FOLDER_PATH;
        [SerializeField] private string _filePath = FILE_PATH;
        [SerializeField] private EnumArray<Files, bool> _startLoadFiles = new(true);

        public string FolderPath => _folderPath;
        public string FilePath => _filePath;
        public IReadOnlyList<bool> LoadFiles => _startLoadFiles.Values;

        public void Dispose() => Resources.UnloadAsset(this);
    }
}
