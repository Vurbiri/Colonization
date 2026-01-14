using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.International;

namespace VurbiriEditor.International
{
	using static CONST;

	internal class LanguageFilesScriptable : ScriptableObject
    {
		private const string META_EXP = ".meta";
        private static LanguageFilesScriptable s_instance;

        [SerializeField] private List<string> _files = new();

        public static LanguageFilesScriptable LoadOrCreate()
        {
            if (s_instance == null)
                s_instance = InitDataCreate.LoadOrCreateScriptable<LanguageFilesScriptable>(LANG_FILES_NAME, LANG_FILES_PATH);
            return s_instance;
        }
        public static void Unload() => ResourcesExt.Unload(ref s_instance);

        public void OnAdded(IEnumerable<int> indexes)
		{
			foreach (int index in indexes)
				_files[index] = string.Empty;
		}

		public void Load()
		{
			_files = JsonResources.Load<List<string>>(FILE_FILES);
			LanguageData.SetFiles(_files);
			EditorUtility.SetDirty(this);
            //AssetDatabase.SaveAssets();
        }

        public void Apply()
		{
			if (Save())
			{
				AssetDatabase.Refresh();
				Debug.Log($"Saved <i>{FILE_FILES_PATH}</i>");
				LanguageData.SetFiles(_files);
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
				var folders = JsonResources.Load<List<LanguageType>>(FILE_LANG)
					.Select(l => FileUtil.GetPhysicalPath(RESOURCES_FOLDER.Concat(l.Folder, "/"))).GroupBy(f => f).Select(g => g.First()).ToArray();

				int count = MathI.Min(_files.Count, LanguageData.fileCount);
				for (int i = 0; i < count; i++)
				{
					if (_files[i] != LanguageData.fileNames[i])
					{
						foreach (var folder in folders)
						{
							string src = folder.Concat(LanguageData.fileNames[i], JSON_EXP);
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
			if (LanguageData.fileCount == 0)
				_files.Add("Main");
		}
	}
}
