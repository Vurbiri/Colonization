using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(Vurbiri.Colonization.PlayerAbilitiesScriptable), true), CanEditMultipleObjects]
    internal class PlayerAbilitiesScriptableEditor : AEditorGetVE<PlayerAbilitiesScriptableEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;

        public override VisualElement CreateInspectorGUI() => _treeAssetList.CloneTree();
    }
}
