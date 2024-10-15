using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(PricesScriptable), true), CanEditMultipleObjects]
    internal class PricesScriptableEditor : AEditorGetVE<PricesScriptableEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;

        private const string NAME_PROPERTY = "_edifices", NAME_ARRAY = "_values";

        protected override VisualElement Create(SerializedObject serializedObject)
        {
            SerializedProperty propertyValues = serializedObject.FindProperty(NAME_PROPERTY).FindPropertyRelative(NAME_ARRAY);

            VisualElement root = _treeAssetList.CloneTree();

            for (int i = EdificeId.Shrine; i < EdificeId.Count; i++)
            {
                root.Q<PropertyField>(EdificeId.Names[i]).BindProperty(propertyValues.GetArrayElementAtIndex(i));
            }

            return root;
        }
    }
}
