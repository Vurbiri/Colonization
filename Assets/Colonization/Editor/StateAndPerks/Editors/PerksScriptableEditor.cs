using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(PlayerPerksScriptable), true), CanEditMultipleObjects]
    internal class PerksScriptableEditor : AEditorGetVE<PerksScriptableEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;
        [SerializeField] private VisualTreeAsset _treeAssetItem;

        public override VisualElement CreateInspectorGUI()
        {
            SerializedProperty perksProperty = serializedObject.FindProperty("_perks");
            VisualElement root = _treeAssetList.CloneTree();

            var list = root.Q<ListView>("Perks");
            list.makeItem = MakeItem;
            list.itemIndexChanged += (x,y) => UpdateId();
            list.itemsAdded += (e) => UpdateId();
            list.itemsRemoved += (e) => UpdateId();

            return root;

            #region Local: MakeItem(), UpdateId()
            //=================================
            VisualElement MakeItem()
            {
                VisualElement item = _treeAssetItem.CloneTree();

                var id = item.Q<IntegerField>("Id");
                var name = item.Q<TextField>("Name");
                var foldout = item.Q<Foldout>("Foldout");
                foldout.text = name.value;

                id.SetEnabled(false);
                name.RegisterValueChangedCallback((e) => foldout.text = e.newValue);


                return item;
            }
            //=================================
            void UpdateId()
            {
                serializedObject.Update();
                for (int i = 0; i < perksProperty.arraySize; i++)
                    perksProperty.GetArrayElementAtIndex(i).FindPropertyRelative("_id").intValue = i;
                serializedObject.ApplyModifiedProperties();
            }
            #endregion
        }

       
    }
}
