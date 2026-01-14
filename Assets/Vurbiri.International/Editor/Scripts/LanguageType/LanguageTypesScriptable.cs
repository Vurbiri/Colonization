using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.International;
using static Vurbiri.JsonResources;

namespace VurbiriEditor.International
{
	using static CONST;

	internal class LanguageTypesScriptable : AGetOrCreateScriptableObject<LanguageTypesScriptable>
	{
		#pragma warning disable 414
		[SerializeField] private bool _readonly = true;
		#pragma warning restore 414
		[SerializeField] private List<LanguageType> _languageTypes = new();

		public void Load()
		{
			_languageTypes = Load<List<LanguageType>>(FILE_LANG);
		}

		public void Save()
		{
			File.WriteAllText(FileUtil.GetPhysicalPath(FILE_LANG_PATH), JsonConvert.SerializeObject(_languageTypes, Formatting.Indented), utf8WithoutBom);
			AssetDatabase.Refresh();
			Debug.Log($"Saved <i>{FILE_LANG_PATH}</i>");

			LanguageData.folder = _languageTypes[0].Folder;
		}

		public bool Contains(SystemLanguage id)
		{
			int i = _languageTypes.Count;
			while (i --> 0 && _languageTypes[i].Id != id);
			return i >= 0;
		}

		public static LanguageTypesScriptable GetOrCreateSelf() => GetOrCreateSelf(LANG_TYPES_NAME, LANG_TYPES_PATH);
		public static SerializedObject GetSerializedSelf() => new(GetOrCreateSelf(LANG_TYPES_NAME, LANG_TYPES_PATH));

		private void OnValidate()
		{
			if (_languageTypes.Count == 0)
				_languageTypes.Add(new(SystemLanguage.Russian, "ru", "Русский", "Russian", "Banner"));
		}
	}
}
