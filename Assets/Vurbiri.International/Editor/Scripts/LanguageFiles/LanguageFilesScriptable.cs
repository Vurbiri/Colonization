//Assets\Vurbiri.International\Editor\Scripts\LanguageFiles\LanguageFilesScriptable.cs
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Vurbiri.International.Editor
{
    using static CONST;

    internal class LanguageFilesScriptable : AGetOrCreateScriptableObject<LanguageFilesScriptable>
    {
        private const string ENUM_KEYWORD = @"#ENUM#";
        private const string ENUM_TMP = "FilesTemplate";
        private const string ENUM_PATH = FOLDER + "Runtime/Files.cs";
        private const string SPACE = "        ";
                
        [SerializeField] private List<string> _files;

        private string _template;

        public void Init()
        {
            _template = Resources.Load<TextAsset>(ENUM_TMP).text;

            _files = new(Enum<Files>.Names);
        }

        public void Apply()
        {
            if (SaveEnum())
            {
                Debug.Log("Saved");
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogWarning("Save error");
            }
        }

        public bool SaveEnum()
        {
            StringBuilder sb = new(_files.Count * 12);
            for (int i = 0; i < _files.Count; i++)
            {
                if (!CheckValue(_files[i], i))
                {
                    _files.RemoveAt(i);
                    return false;
                }

                sb.Append(SPACE);
                sb.Append(_files[i]);
                if (i < _files.Count - 1)
                    sb.AppendLine(",");
            }

            File.WriteAllText(Application.dataPath.Concat(ENUM_PATH), _template.Replace(ENUM_KEYWORD, sb.ToString()), utf8WithoutBom);

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
                _files.Add(((Files)0).ToString());
        }

        public static LanguageFilesScriptable GetOrCreateSelf() => GetOrCreateSelf(LANG_FILES_NAME, LANG_FILES_PATH);
        public static SerializedObject GetSerializedSelf() => new(GetOrCreateSelf(LANG_FILES_NAME, LANG_FILES_PATH));
    }
}
