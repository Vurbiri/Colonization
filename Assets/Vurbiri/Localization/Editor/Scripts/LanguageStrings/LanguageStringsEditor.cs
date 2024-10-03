using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VurbiriEditors;

namespace Vurbiri.Localization.Editors
{
    [CustomEditor(typeof(LanguageStringsScriptable), true)]
    internal class LanguageStringsEditor : AEditorGetVE<LanguageStringsEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            var strings = LanguageStringsScriptable.GetOrCreateSelf();
            strings.Initialize();

            VisualElement root = _treeAsset.CloneTree();

            var list = root.Q<ListView>("Strings");
            list.makeItem = LanguageRecordEditor.GetVisualElement;
            list.headerTitle = strings.LoadFile;
            list.itemsAdded += strings.OnAdded;

            root.Q<Button>("Load").clicked += () => list.headerTitle = strings.Load();
            root.Q<Button>("Save").clicked += strings.Save;
            root.Q<Button>("Unload").clicked += () => { list.headerTitle = string.Empty; strings.Reset(); };

            return root;
        }
    }
}
