using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VurbiriEditor;

namespace Vurbiri.Localization.Editors
{
    [CustomEditor(typeof(LanguageRecord), true)]
    public class LanguageRecordEditor : AEditorGetVE<LanguageRecordEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;
        [SerializeField] private VisualTreeAsset _treeAssetItem;

        protected override VisualElement Create(SerializedObject serializedObject)
        {
            VisualElement root = _treeAssetList.CloneTree();

            var list = root.Q<ListView>("Values");
            list.makeItem = _treeAssetItem.CloneTree;

            return root;
        }
    }
}
