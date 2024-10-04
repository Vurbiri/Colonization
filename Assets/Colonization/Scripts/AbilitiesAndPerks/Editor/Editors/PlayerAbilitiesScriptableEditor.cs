using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VurbiriEditors;

namespace Vurbiri.Colonization
{
    [CustomEditor(typeof(PlayerAbilitiesScriptable), true), CanEditMultipleObjects]
    internal class PlayerAbilitiesScriptableEditor : AEditorGetVE<PlayerAbilitiesScriptableEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;

        public override VisualElement CreateInspectorGUI() => _treeAssetList.CloneTree();
    }
}
