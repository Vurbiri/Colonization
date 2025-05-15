//Assets\Vurbiri.International\Editor\Scripts\LanguageFiles\LanguageFilesEditor.cs
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VurbiriEditor;

namespace Vurbiri.International.Editor
{
    [CustomEditor(typeof(LanguageFilesScriptable), true)]
    internal class LanguageFilesEditor : AEditorGetVE<LanguageFilesEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            var settings = LanguageFilesScriptable.GetOrCreateSelf();
            settings.Init();
            var root = _treeAsset.CloneTree();

            var list = root.Q<ListView>("Files");
            list.makeItem = MakeItem;

            root.Q<Button>("Apply").clicked += settings.Apply;

            return root;
        }

        private VisualElement MakeItem()
        {
            TextField field = new()
            {
                label = "File:"
            };

            return field;
        }
    }
}
