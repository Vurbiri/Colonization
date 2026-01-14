using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.International;

namespace VurbiriEditor.International
{
	using static CONST;

	internal class LanguageTypesScriptable : ScriptableObject
    {
		private static LanguageTypesScriptable s_instance;
		
		#pragma warning disable 414
		[SerializeField] private bool _readonly = true;
		#pragma warning restore 414
		[SerializeField] private List<LanguageType> _languageTypes;

		public static LanguageTypesScriptable LoadOrCreate()
		{
			if(s_instance == null)
                s_instance = InitDataCreate.LoadOrCreateScriptable<LanguageTypesScriptable>(LANG_TYPES_NAME, LANG_TYPES_PATH);
			return s_instance;
		}
        public static void Unload() => ResourcesExt.Unload(ref s_instance);

        public void Load()
		{
			_languageTypes = JsonResources.Load<List<LanguageType>>(FILE_LANG);

            EditorUtility.SetDirty(this);
        }

		public void Save()
		{
			if (_languageTypes.Count > 0)
			{
				File.WriteAllText(FileUtil.GetPhysicalPath(FILE_LANG_PATH), JsonConvert.SerializeObject(_languageTypes, Formatting.Indented), utf8WithoutBom);
				AssetDatabase.Refresh();
				Debug.Log($"Saved <i>{FILE_LANG_PATH}</i>");

				LanguageData.folder = _languageTypes[0].Folder;
			}
		}

		private void OnValidate()
		{
            _languageTypes ??= new();
            if (_languageTypes.Count == 0)
				_languageTypes.Add(new(SystemLanguage.Russian, "ru", "Русский", "Russian", "Banner"));
		}
	}
}
