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
        [SerializeField] private VisualTreeAsset _treeSkillsVT;

        private const string PROP_SETTINGS = "_settings", PROP_ARRAY = "_values", PROP_ID = "_id", PROP_SKILLS = "_skills";
        private const string NAME_CONTAINER = "Container", NAME_LABEL = "Label", NAME_SKILLS = "Skills";

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = _treeWarriorsSettingsScriptable.CloneTree();
            VisualElement container = root.Q<VisualElement>(NAME_CONTAINER);

            SerializedProperty propertyValues = serializedObject.FindProperty(PROP_SETTINGS).FindPropertyRelative(PROP_ARRAY);

            SerializedProperty propertyValue, propertySkills;
            VisualElement element;
            for (int i = 0; i < WarriorId.Count; i++)
            {
                propertyValue = propertyValues.GetArrayElementAtIndex(i);
                propertyValue.FindPropertyRelative(PROP_ID).intValue = i;
                propertySkills = propertyValue.FindPropertyRelative(PROP_SKILLS);

                element = _treeWarriorSettings.Instantiate(propertyValue.propertyPath);
                element.Q<Foldout>(NAME_LABEL).text = WarriorId.GetName(i);
                element.Q<VisualElement>(NAME_SKILLS).Add(_treeSkillsVT.Instantiate(propertySkills.propertyPath));
                container.Add(element);
            }

            return root;
        }
    }
}
