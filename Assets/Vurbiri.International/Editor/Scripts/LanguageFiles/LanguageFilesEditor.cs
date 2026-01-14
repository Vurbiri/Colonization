using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace VurbiriEditor.International
{
	[CustomEditor(typeof(LanguageFilesScriptable), true)]
	internal class LanguageFilesEditor : AEditorGetVE<LanguageFilesEditor>
	{
		[SerializeField] private VisualTreeAsset _treeAsset;

		private int _index = 0;
		private ListView _list;

		public static void Load(string searchContext, VisualElement rootElement)
		{
            rootElement.Add(CreateEditorAndBind(LanguageFilesScriptable.LoadOrCreate(), out s_self));
		}
        public static void Unload()
        {
            Destroy();
            LanguageFilesScriptable.Unload();
        }

        public override VisualElement CreateInspectorGUI()
		{
			var settings = LanguageFilesScriptable.LoadOrCreate();

			var root = _treeAsset.CloneTree();

			root.Q<Label>("Label").text = CONST.PROJECT_FILES_LABEL;

			_index = 0;
			_list = root.Q<ListView>("Files");
			_list.makeItem = OnMakeItem;
			_list.itemsRemoved += ReIndex;
			_list.itemsAdded += ReIndex;
			_list.itemsAdded += settings.OnAdded;

			root.Q<Button>("Load").clicked += settings.Load;
            root.Q<Button>("Apply").clicked += settings.Apply;

			settings.Load();
			serializedObject.Update();

            return root;
		}

		private VisualElement OnMakeItem()
		{
			TextField field = new() { label = $"File {_index++,2}" };
			return field;
		}

		private void ReIndex(IEnumerable<int> indexes)
		{
			int index = 0;
			foreach (var field in _list.Children().Cast<TextField>())
				field.label = $"File {index++,2}";
		}
	}
}
