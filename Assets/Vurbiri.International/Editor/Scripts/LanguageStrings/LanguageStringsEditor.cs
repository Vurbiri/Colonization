using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VurbiriEditor;

namespace Vurbiri.International.Editor
{
    [CustomEditor(typeof(LanguageStringsScriptable), true)]
    internal class LanguageStringsEditor : AEditorGetVE<LanguageStringsEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            var strings = LanguageStringsScriptable.GetOrCreateSelf();
            strings.Init();

            VisualElement root = _treeAsset.CloneTree();

            root.Q<Label>("Label").text = CONST.PROJECT_STRING_LABEL;

            root.Q<DropdownField>("File").choices = new(LanguageFiles.names);

            var list = root.Q<ListView>("Strings");
            list.makeItem = LanguageRecordEditor.CreateInstanceAndGetVisualElement;
            list.headerTitle = strings.LoadFile;
            list.itemsAdded += strings.OnAdded;
            
            root.Q<Button>("Load").clicked += () => { list.headerTitle = strings.Load(); serializedObject.Update(); };
            root.Q<Button>("Save").clicked += strings.Save;
            root.Q<Button>("Unload").clicked += () => { list.headerTitle = string.Empty; strings.Unload(); serializedObject.Update(); };

            return root;
        }
    }
}
