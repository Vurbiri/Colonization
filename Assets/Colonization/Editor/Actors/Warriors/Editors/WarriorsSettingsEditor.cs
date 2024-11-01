using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization;
using Vurbiri.Colonization.Actors;

namespace VurbiriEditor.Colonization.Actors
{
    [CustomEditor(typeof(WarriorsSettingsScriptable), true)]
    public class WarriorsSettingsEditor : AEditorGetVE<WarriorsSettingsEditor>
    {
        [SerializeField] private VisualTreeAsset _treeWarriorsSettingsScriptable;
        [SerializeField] private VisualTreeAsset _treeWarriorSettings;

        private const string NAME_SETTINGS = "_settings", NAME_ARRAY = "_values", NAME_ID = "_id";
        private const string NAME_CONTAINER = "Container", NAME_LABEL = "Label";

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = _treeWarriorsSettingsScriptable.CloneTree();
            VisualElement container = root.Q<VisualElement>(NAME_CONTAINER);

            SerializedProperty propertyValues = serializedObject.FindProperty(NAME_SETTINGS).FindPropertyRelative(NAME_ARRAY);

            SerializedProperty propertyValue;
            VisualElement element;
            for (int i = 0; i < WarriorId.Count; i++)
            {
                propertyValue = propertyValues.GetArrayElementAtIndex(i);
                propertyValue.FindPropertyRelative(NAME_ID).intValue = i;

                element = _treeWarriorSettings.Instantiate(propertyValue.propertyPath);
                element.Q<Foldout>(NAME_LABEL).text = WarriorId.GetName(i);
                container.Add(element);
            }

            return root;
        }
    }
}
