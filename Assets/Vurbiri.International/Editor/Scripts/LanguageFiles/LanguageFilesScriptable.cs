//Assets\Vurbiri.International\Editor\Scripts\LanguageFiles\LanguageFilesScriptable.cs
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Vurbiri.International.Editor
{
    using static CONST;

    internal class LanguageFilesScriptable : AGetOrCreateScriptableObject<LanguageFilesScriptable>
    {
        [SerializeField] private List<string> _files;

        //private string[] _folders;

        public void Init()
        {
            //_folders = LoadObjectFromResourceJson<List<LanguageType>>(FILE_LANG).Select(l => l.Folder).ToArray();
        }

        public void OnAdded(IEnumerable<int> indexes)
        {
            foreach (int index in indexes)
                _files[index] = string.Empty;
        }

        public void Load()
        {
            _files = new(LanguageFiles.names);
            EditorUtility.SetDirty(this);
        }

        public void Apply()
        {
            if (Save())
            {
                AssetDatabase.Refresh();
                Debug.Log($"Saved <i>{FILE_FILES_PATH}</i>");
                LanguageFiles.Set(_files);
            }
            else
            {
                Debug.LogWarning("Save error");
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public bool Save()
        {
            for (int i = _files.Count - 1; i >= 0; i--)
            {
                if (!CheckValue(_files[i], i))
                    _files.RemoveAt(i);
            }

            if (_files.Count == 0)
                return false;

            File.WriteAllText(FileUtil.GetPhysicalPath(FILE_FILES_PATH), JsonConvert.SerializeObject(_files, Formatting.Indented), utf8WithoutBom);
            
            return true;

            #region Local: CheckValue(..)
            //=================================
            bool CheckValue(string value, int index)
            {
                if (string.IsNullOrEmpty(value))
                    return false;

                for (int i = 0; i < index; i++)
                    if (value == _files[i])
                        return false;

                return true;
            }
            #endregion
        }

        private void OnValidate()
        {
            _files ??= new();
            if (_files.Count == 0)
                _files.Add("Main");
        }

        public static LanguageFilesScriptable GetOrCreateSelf() => GetOrCreateSelf(LANG_FILES_NAME, LANG_FILES_PATH);
        public static SerializedObject GetSerializedSelf() => new(GetOrCreateSelf(LANG_FILES_NAME, LANG_FILES_PATH));
    }
}
