using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vurbiri.Colonization
{
    [CustomEditor(typeof(PlayerAbilitiesScriptable), true), CanEditMultipleObjects]
    internal class PlayerAbilitiesScriptableEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;
        [SerializeField] private VisualTreeAsset _treeAssetItem;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = _treeAssetList.CloneTree();

            var list = root.Q<ListView>("Abilities");
            list.makeItem = _treeAssetItem.CloneTree;

            return root;
        }
    }
}
