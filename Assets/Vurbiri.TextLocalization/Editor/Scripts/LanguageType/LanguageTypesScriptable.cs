//Assets\Vurbiri.TextLocalization\Editor\Scripts\LanguageType\LanguageTypesScriptable.cs
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static Vurbiri.Storage;

namespace Vurbiri.TextLocalization.Editor
{
    using static CONST;

    //[CreateAssetMenu(fileName = LANG_TYPES_NAME, menuName = "Vurbiri/Localization/LanguageTypes", order = 51)]
    internal class LanguageTypesScriptable : AGetOrCreateScriptableObject<LanguageTypesScriptable>
    {
        [SerializeField] private bool _auto = true;
        [SerializeField] private List<LanguageType> _languageTypes = new();

        public void Load()
        {
            if (LoadObjectFromResourceJson(CONST_L.FILE_LANG, out _languageTypes))
                _languageTypes.Sort();
        }

        public void Save()
        {
            File.WriteAllText(Application.dataPath.Concat(CONST_L.FILE_PATH), JsonConvert.SerializeObject(_languageTypes, Formatting.Indented));
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        public static LanguageTypesScriptable GetOrCreateSelf() => GetOrCreateSelf(LANG_TYPES_NAME, LANG_TYPES_PATH);
        public static SerializedObject GetSerializedSelf() => new(GetOrCreateSelf(LANG_TYPES_NAME, LANG_TYPES_PATH));

        private void OnValidate()
        {
            if (_languageTypes.Count == 0)
                _languageTypes.Add(new(0, "ru", "Русский", "Russian", "Banner"));


            for (int i = 0; i < _languageTypes.Count; i++)
                if(_languageTypes[i].Id != i)
                    _languageTypes[i] = new(i, _languageTypes[i]);
        }

        public void EmptyMethod()
        {
            if (_auto)
                Debug.Log("Чтобы не ругался Editor");
        }
    }
}
