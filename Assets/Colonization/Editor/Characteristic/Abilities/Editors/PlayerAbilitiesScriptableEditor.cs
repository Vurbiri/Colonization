//Assets\Colonization\Editor\Characteristic\Abilities\Editors\PlayerAbilitiesScriptableEditor.cs
namespace VurbiriEditor.Colonization.Characteristics
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(Vurbiri.Colonization.Characteristics.PlayerAbilitiesScriptable), true), CanEditMultipleObjects]
    internal class PlayerAbilitiesScriptableEditor : AEditorGetVE<PlayerAbilitiesScriptableEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;

        public override VisualElement CreateInspectorGUI() => _treeAssetList.CloneTree();
    }
}
