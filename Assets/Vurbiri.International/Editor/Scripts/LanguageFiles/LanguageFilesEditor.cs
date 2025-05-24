using System.Collections.Generic;
using System.Linq;
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

        private int _index = 0;
        private ListView _list;

        public override VisualElement CreateInspectorGUI()
        {
            var settings = LanguageFilesScriptable.GetOrCreateSelf();

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
