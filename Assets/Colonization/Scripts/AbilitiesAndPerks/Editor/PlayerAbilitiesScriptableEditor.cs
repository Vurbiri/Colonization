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
        //[SerializeField] private VisualTreeAsset _treeAssetItem;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = _treeAssetList.CloneTree();

            //var list = root.Q<ListView>("Abilities");
            //list.makeItem = _treeAssetItem.CloneTree;

            return root;
        }
    }
}
