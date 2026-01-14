using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace VurbiriEditor.International
{
	[CustomEditor(typeof(LanguageStringsScriptable), true)]
	internal class LanguageStringsEditor : AEditorGetVE<LanguageStringsEditor>
	{
		[SerializeField] private VisualTreeAsset _treeAsset;

		public static void Load(string searchContext, VisualElement rootElement)
		{
			rootElement.Add(CreateEditorAndBind(LanguageStringsScriptable.LoadOrCreate(), out s_self));
		}
		public static void Unload()
		{
			Destroy();
			LanguageStringsScriptable.Unload();
		}

		public override VisualElement CreateInspectorGUI()
		{
			var strings = LanguageStringsScriptable.LoadOrCreate();
			strings.Init(); serializedObject.Update();

            VisualElement root = _treeAsset.CloneTree();

			root.Q<Label>("Label").text = CONST.PROJECT_STRING_LABEL;

			var list = root.Q<ListView>("Strings");
            list.makeItem = LanguageRecordEditor.CreateInstanceAndGetVisualElement;
            list.headerTitle = strings.LoadFile;
            list.itemsAdded += strings.OnAdded;

            var dropdown = root.Q<DropdownField>("File");
			dropdown.choices = new(LanguageData.fileNames);
			//dropdown.RegisterValueChangedCallback(evt => { list.headerTitle = strings.Load(); serializedObject.Update(); });
						
			root.Q<Button>("Load").clicked += () => { list.headerTitle = strings.Load(); serializedObject.Update(); };
			root.Q<Button>("Save").clicked += strings.Save;

			return root;
		}
	}
}
