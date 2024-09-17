using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Localization
{
    using static CONST;

    //[CreateAssetMenu(fileName = "LocalizationSettings", menuName = "Vurbiri/Localization/Settings", order = 51)]
    public class SettingsScriptable : ScriptableObject
    {
        
        [SerializeField] private string _folderPath = FOLDER_PATH;
        [SerializeField] private string _folder = FOLDER;
        [SerializeField] private string _filePath = FILE_PATH;
        [SerializeField] private string _languagesFile = FILE_LANG;
        [SerializeField] private EnumArray<Files, bool> _loadFiles = new(true);

        public string FolderPath => _folderPath;
        public string Folder => _folder;
        public string FilePath => _filePath;
        public string LanguageFile => _languagesFile;
        public IReadOnlyList<bool> LoadFiles => _loadFiles.Values;
    }
}
