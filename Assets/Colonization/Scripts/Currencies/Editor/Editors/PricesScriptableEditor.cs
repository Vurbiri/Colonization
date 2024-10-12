using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(Vurbiri.Colonization.PricesScriptable), true), CanEditMultipleObjects]
    internal class PricesScriptableEditor : AEditorGetVE<PricesScriptableEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;

        public override VisualElement CreateInspectorGUI() => _treeAssetList.CloneTree();
    }
}
