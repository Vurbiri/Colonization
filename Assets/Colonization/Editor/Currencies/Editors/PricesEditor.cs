using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomEditor(typeof(Prices), true)]
    internal class PricesEditor : AEditorGetVE<PricesEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;

        private const string NAME_ENDIFACE = "_edifices", NAME_WARRIORS = "_warriors", NAME_ARRAY = "_values";

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = _treeAssetList.CloneTree();

            SerializedProperty propertyValues = serializedObject.FindProperty(NAME_ENDIFACE).FindPropertyRelative(NAME_ARRAY);

            for (int i = EdificeId.Shrine; i < EdificeId.Count; i++)
            {
                root.Q<PropertyField>(EdificeId.Names_Ed[i]).BindProperty(propertyValues.GetArrayElementAtIndex(i));
            }

            propertyValues = serializedObject.FindProperty(NAME_WARRIORS).FindPropertyRelative(NAME_ARRAY);

            for (int i = 0; i < WarriorId.Count; i++)
            {
                root.Q<PropertyField>(WarriorId.Names_Ed[i]).BindProperty(propertyValues.GetArrayElementAtIndex(i));
            }

            return root;
        }
    }
}
