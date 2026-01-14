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

	internal class LanguageStringsScriptable : ScriptableObject
	{
		private static LanguageStringsScriptable s_instance;

		[SerializeField] private string _selectFile = "Main";
        [SerializeField] private string _loadFile = string.Empty;
		[SerializeField] private List<LanguageRecord> _strings;

		private List<LanguageType> _languages;
		private string[] _names;
		private int _count;

		public string LoadFile => _loadFile;

		public static LanguageStringsScriptable LoadOrCreate()
		{
			if (s_instance == null)
				s_instance = InitDataCreate.LoadOrCreateScriptable<LanguageStringsScriptable>(LANG_STRING_NAME, LANG_STRING_PATH);
			return s_instance;
		}
		public static void Unload() => ResourcesExt.Unload(ref s_instance);

		public void Init()
		{
			_languages = JsonResources.Load<List<LanguageType>>(CONST_L.LANG_FILE);

			for (int i = _languages.Count - 1; i >= 0; i--)
				if (!CheckFolder(_languages[i].Folder, i))
					_languages.RemoveAt(i);

			_count = _languages.Count;
			_names = new string[_count];
			for (int i = 0; i < _count; i++)
				_names[i] = _languages[i].Folder.Concat(" (", _languages[i].Name, ")");

            if (_loadFile != _selectFile)
                Load();

            #region Local: CheckFolder(..)
            //=================================
            bool CheckFolder(string folder, int index)
			{
				if (string.IsNullOrEmpty(folder))
					return false;

				for (int i = 0; i < index; i++)
					if (folder == _languages[i].Folder)
						return false;

				return true;
			}
			#endregion
		}

		public void OnAdded(IEnumerable<int> indexes)
		{
			if (string.IsNullOrEmpty(_loadFile))
			{
				_strings.Clear();
				return;
			}

			LanguageRecord record;
			foreach (int index in indexes)
			{
				record = new(string.Empty);
				for (int i = 0; i < _count; i++)
					record.Add(_names[i], string.Empty);

				_strings[index] = record;
			}
		}

        public string Load()
		{
            _loadFile = _selectFile;

            Dictionary<string, string>[] strings = new Dictionary<string, string>[_count];

            string path, folder = FileUtil.GetPhysicalPath(RESOURCES_FOLDER);
            int idMaxLength = -1, maxLength = -1;
            for (int i = 0; i < _count; i++)
            {
                path = Path.Combine(folder, _languages[i].Folder, _selectFile.Concat(JSON_EXP));

                if (File.Exists(path))
                    strings[i] = JsonResources.Load<Dictionary<string, string>>(Path.Combine(_languages[i].Folder, _selectFile));
                else
                    strings[i] = new();

                if (strings[i].Count > maxLength)
                {
                    maxLength = strings[i].Count;
                    idMaxLength = i;
                }
            }

            _strings = new();

            LanguageRecord record;
            foreach (var key in strings[idMaxLength].Keys)
            {
                record = new(key);

                for (int i = 0; i < _count; i++)
                {
                    if (!strings[i].TryGetValue(key, out string text))
                        text = string.Empty;

                    record.Add(_names[i], text);
                }

                _strings.Add(record);
            }

            EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();

			LanguageData.CreateKeys(_loadFile);

            return _loadFile;
		}

		public void Save()
		{
			if (string.IsNullOrEmpty(_loadFile))
				return;

			Dictionary<string, string>[] strings = new Dictionary<string, string>[_count];

			for (int i = 0; i < _count; i++)
				strings[i] = new(_strings.Count);

			string key;
			foreach (var str in _strings)
			{
				key = str.Key;
				if (string.IsNullOrEmpty(key))
					continue;

				for (int i = 0; i < _count; i++)
					strings[i].Add(key, str.GetText(i));
			}

			string folder = FileUtil.GetPhysicalPath(RESOURCES_FOLDER);
			for (int i = 0; i < _count; i++)
			{
				FileInfo fileInfo = new(Path.Combine(folder, _languages[i].Folder, _loadFile.Concat(JSON_EXP)));
				if (!fileInfo.Exists)
					fileInfo.Directory.Create();

				File.WriteAllText(fileInfo.FullName, JsonConvert.SerializeObject(strings[i], Formatting.Indented), utf8WithoutBom);
				Debug.Log($"Saved <i>{fileInfo.FullName}</i>");
			}

			EditorUtility.SetDirty(this);
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();

			LanguageData.CreateKeys(_loadFile);
		}

        private void OnValidate()
        {
            if(string.IsNullOrEmpty(_selectFile))
                _selectFile = "Main";
        }
    }
}
