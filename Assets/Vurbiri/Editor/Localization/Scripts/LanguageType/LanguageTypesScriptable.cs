using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static Vurbiri.Storage;

namespace Vurbiri.Localization.Editors
{
    using static CONST;

    //[CreateAssetMenu(fileName = LANG_TYPES_NAME, menuName = "Vurbiri/Localization/LanguageTypes", order = 51)]
    internal class LanguageTypesScriptable : AGetOrCreateScriptableObject<LanguageTypesScriptable>
    {
        [SerializeField] private bool _auto = true;
        [SerializeField] private List<LanguageType> _languageTypes = new();

        public void Load()
        {
            using SettingsScriptable settings = ProjectSettingsScriptable.GetCurrentSettings();
            if (settings != null && LoadObjectFromResourceJson(Path.Combine(settings.Folder, settings.LanguageFile), out _languageTypes))
                _languageTypes.Sort();
        }

        public void Save()
        {
            using SettingsScriptable settings = ProjectSettingsScriptable.GetCurrentSettings();

            File.WriteAllText(Application.dataPath.Concat(settings.FilePath), JsonConvert.SerializeObject(_languageTypes));
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        public static LanguageTypesScriptable GetOrCreateSelf() => GetOrCreateSelf(LANG_TYPES_NAME, LANG_TYPES_PATH);
        public static SerializedObject GetSerializedSelf() => new(GetOrCreateSelf(LANG_TYPES_NAME, LANG_TYPES_PATH));

        private void OnValidate()
        {
            for (int i = 0; i < _languageTypes.Count; i++)
                _languageTypes[i].Id = i;
        }

        public void EmptyMethod()
        {
            if (_auto)
                Debug.Log("Чтобы в консоль не ругалась");
        }
    }
}
