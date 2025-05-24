using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Vurbiri.Storage;

namespace Vurbiri.International.Editor
{
    using static CONST;

    internal class LanguageFilesScriptable : AGetOrCreateScriptableObject<LanguageFilesScriptable>
    {
        private const string META_EXP = ".meta";

        [SerializeField] private List<string> _files;

        public void OnAdded(IEnumerable<int> indexes)
        {
            foreach (int index in indexes)
                _files[index] = string.Empty;
        }

        public void Load()
        {
            _files = LoadObjectFromResourceJson<List<string>>(FILE_FILES);
            LanguageFiles.Set(_files);
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
                if (!CheckValue(_files[i], i))
                    _files.RemoveAt(i);

            if (_files.Count == 0)
                return false;

            Rename();

            File.WriteAllText(FileUtil.GetPhysicalPath(FILE_FILES_PATH), JsonConvert.SerializeObject(_files, Formatting.Indented), utf8WithoutBom);
            
            return true;

            #region Local: CheckValue(..), Rename()
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
            //=================================
            void Rename()
            {
                var folders = LoadObjectFromResourceJson<List<LanguageType>>(FILE_LANG)
                    .Select(l => FileUtil.GetPhysicalPath(OUT_RESOURCE_FOLDER.Concat(l.Folder, "/"))).GroupBy(f => f).Select(g => g.First()).ToArray();

                int count = Mathf.Min(_files.Count, LanguageFiles.count);
                for (int i = 0; i < count; i++)
                {
                    if (_files[i] != LanguageFiles.names[i])
                    {
                        foreach (var folder in folders)
                        {
                            string src = folder.Concat(LanguageFiles.names[i], JSON_EXP);
                            string dst = folder.Concat(_files[i], JSON_EXP);
                            
                            if (File.Exists(src))
                            {
                                FileUtil.MoveFileOrDirectory(src, dst);
                                FileUtil.MoveFileOrDirectory(src.Concat(META_EXP), dst.Concat(META_EXP));
                                Debug.Log($"Moved <i>{src}</i> <b>--></b> <i>{dst}</i>");
                            }
                        }
                    }
                }
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
