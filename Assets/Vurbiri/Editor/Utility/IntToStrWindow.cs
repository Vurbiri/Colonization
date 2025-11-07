using Newtonsoft.Json;
using System;
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

        private static readonly Type s_type = typeof(Settings);
        private static readonly FileInfo s_file = new(Application.dataPath.Concat(IntToStr.Settings.RESOURCE, Settings.PATH, ".json"));

		private Settings _current, _cache;

        [MenuItem(MENU, false, 19)]
		private static void ShowWindow()
		{
			GetWindow<IntToStrWindow>(true, NAME).minSize = new(300, 85);
		}
		
		private void OnEnable()
		{
			if(s_file.Exists)
                _cache = _current = (Settings)JsonConvert.DeserializeObject(File.ReadAllText(s_file.FullName, utf8WithoutBom), s_type);
        }
		
		private void OnGUI()
		{
			BeginWindows();
			{
				Space(10f);
				LabelField("Settings", STYLES.H1);
                Space(10f);
                Rect position = BeginVertical(GUI.skin.box);
					VEditorGUI.MinMaxSlider(position, ref _current.min, ref _current.max, Settings.MIN_LIMIT, Settings.MAX_LIMIT);
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

                string json = JsonConvert.SerializeObject(_current, s_type, Formatting.Indented, null);
                File.WriteAllText(s_file.FullName, json, utf8WithoutBom);

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();

                Debug.Log($"<color={Color.green.ToHex()}><b>[IntToStr]</b></color> Settings file is saved: <b><color={Color.cyan.ToHex()}>{s_file.FullName}</color></b>  ");
            }
            
        }
    }
}