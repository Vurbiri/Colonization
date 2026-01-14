using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUILayout;
using static VurbiriEditor.CONST_EDITOR;
using Settings = Vurbiri.IntToStr.Settings;

namespace VurbiriEditor
{
	public class IntToStrWindow : EditorWindow
	{
		private const string NAME = "IntToStr", MENU = MENU_PATH + NAME;
		private const int OFFSET = 128, L_LIMIT = 32, R_LIMIT = (OFFSET << 1) - L_LIMIT;
		private static readonly FileInfo s_file = new("/Assets/Resources/IntToStr/Settings.json");

		private Settings _current, _cache;
		private int _min, _max;

		static IntToStrWindow()
		{
			if (EUtility.TryGetResourcePath(Settings.PATH, out string path))
				s_file = new(path);
		}

		[MenuItem(MENU, false, 19)]
		private static void ShowWindow()
		{
			GetWindow<IntToStrWindow>(true, NAME).minSize = new(300, 85);
		}
		
		private void OnEnable()
		{
			if (s_file.Exists)
				_cache = _current = (Settings)JsonResources.Load(Settings.PATH, typeof(Settings));

			_min = _current.min - OFFSET;
			_max = _current.max + OFFSET;
		}
		
		private void OnGUI()
		{
			int delta = -(_min - _current.min);
			if (delta < L_LIMIT || delta > R_LIMIT)
				_min = _current.min - OFFSET;

			delta = _max - _current.max;
			if (delta < L_LIMIT || delta > R_LIMIT)
				_max = _current.max + OFFSET;

			BeginWindows();
			{
				Space(10f);
				LabelField("Settings", STYLES.H1);
				Space(10f);
				Rect position = BeginVertical(GUI.skin.box);
					VEditorGUI.MinMaxSlider(position, ref _current.min, ref _current.max, _min, _max);
				EndVertical();
			}
			EndWindows();
		}
		
		private void OnDisable()
		{
			if(!_current.Equals(_cache))
			{
				if (!s_file.Directory.Exists)
					s_file.Directory.Create();

				string json = JsonConvert.SerializeObject(_current, typeof(Settings), Formatting.Indented, null);
				File.WriteAllText(s_file.FullName, json, utf8WithoutBom);

				AssetDatabase.Refresh();
				AssetDatabase.SaveAssets();

				Debug.Log($"<color={Color.green.ToHex()}><b>[IntToStr]</b></color> Settings file is saved: <b><color={Color.cyan.ToHex()}>{s_file.FullName}</color></b>");
			}
		}
	}
}