using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(Vurbiri.Colonization.PlayerStatesScriptable), true), CanEditMultipleObjects]
    internal class PlayerStatesScriptableEditor : AEditorGetVE<PlayerStatesScriptableEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;

        protected override VisualElement Create(SerializedObject serializedObject) => _treeAssetList.CloneTree();
    }
}
