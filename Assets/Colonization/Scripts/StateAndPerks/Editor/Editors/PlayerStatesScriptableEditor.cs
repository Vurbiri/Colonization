using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VurbiriEditors;

namespace Vurbiri.Colonization
{
    [CustomEditor(typeof(PlayerStatesScriptable), true), CanEditMultipleObjects]
    internal class PlayerStatesScriptableEditor : AEditorGetVE<PlayerStatesScriptableEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;

        public override VisualElement CreateInspectorGUI() => _treeAssetList.CloneTree();
    }
}
