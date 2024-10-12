using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(Vurbiri.Colonization.PlayerStatesScriptable), true), CanEditMultipleObjects]
    internal class PlayerStatesScriptableEditor : AEditorGetVE<PlayerStatesScriptableEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;

        public override VisualElement CreateInspectorGUI() => _treeAssetList.CloneTree();
    }
}
