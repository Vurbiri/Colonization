using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(PerksScriptable), true), CanEditMultipleObjects]
    public class PerksScriptableEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;
        [SerializeField] private VisualTreeAsset _treeAssetItem;


        protected SerializedProperty _perksProperty;

        protected virtual void OnEnable()
        {
            _perksProperty = serializedObject.FindProperty("_perks");
        }

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

            #region Local: LoadLangs(), MakeItem(), OnItemIndexChanged(...)
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
                //base.OnInspectorGUI();
                serializedObject.Update();
                for (int i = 0; i < _perksProperty.arraySize; i++)
                    _perksProperty.GetArrayElementAtIndex(i).FindPropertyRelative("_id").intValue = i;
                serializedObject.ApplyModifiedProperties();
            }
            #endregion
        }

       
    }
}
