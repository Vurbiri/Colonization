using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(Vurbiri.Colonization.PricesScriptable), true), CanEditMultipleObjects]
    internal class PricesScriptableEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;

        private const string NAME_PROPERTY = "_edifices", NAME_ARRAY = "_values";

        public override VisualElement CreateInspectorGUI() => Draw(serializedObject);

        public VisualElement Draw(SerializedObject serializedObject)
        {
            SerializedProperty propertyValues = serializedObject.FindProperty(NAME_PROPERTY).FindPropertyRelative(NAME_ARRAY);

            VisualElement root = _treeAssetList.CloneTree();

            for (int i = EdificeId.Shrine; i < EdificeId.Count; i++)
            {
                root.Q<PropertyField>(EdificeId.Names[i]).BindProperty(propertyValues.GetArrayElementAtIndex(i));
            }

            return root;
        }

        public static VisualElement GetVisualElement(SerializedObject serializedObject)
        {
            VisualElement element = CreateInstance<PricesScriptableEditor>().Draw(serializedObject);
            element.Bind(serializedObject);
            return element;
        }
    }
}
