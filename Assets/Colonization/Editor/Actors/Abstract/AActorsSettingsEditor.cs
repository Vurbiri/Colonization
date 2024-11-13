namespace VurbiriEditor.Colonization.Actors
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Vurbiri;

    public abstract class AActorsSettingsEditor<T> : AEditorGetVE<T> where T : AActorsSettingsEditor<T>
    {
        [SerializeField] private VisualTreeAsset _treeActorsSettingsScriptable;
        [SerializeField] private VisualTreeAsset _treeActorSettings;
        [SerializeField] private VisualTreeAsset _treeSkillsVT;

        private const string P_SETTINGS = "_settings", P_ARRAY = "_values", P_ID = "_id", P_SKILLS = "_skills";
        private const string U_CONTAINER = "Container", U_LABEL = "Label", U_SKILLS = "Skills";

        protected VisualElement CreateGUI<TId>(string captionText) where TId : AIdType<TId>
        {
            var root = _treeActorsSettingsScriptable.CloneTree();
            root.Q<Label>(U_LABEL).text = captionText;
            var container = root.Q<VisualElement>(U_CONTAINER);

            SerializedProperty propertyValues = serializedObject.FindProperty(P_SETTINGS).FindPropertyRelative(P_ARRAY);

            SerializedProperty propertyValue, propertySkills;
            VisualElement element;
            for (int i = 0; i < AIdType<TId>.Count; i++)
            {
                propertyValue = propertyValues.GetArrayElementAtIndex(i);
                propertyValue.FindPropertyRelative(P_ID).intValue = i;
                propertySkills = propertyValue.FindPropertyRelative(P_SKILLS);

                element = _treeActorSettings.Instantiate(propertyValue.propertyPath);
                element.Q<Foldout>(U_LABEL).text = AIdType<TId>.GetName(i);
                element.Q<VisualElement>(U_SKILLS).Add(_treeSkillsVT.Instantiate(propertySkills.propertyPath));
                container.Add(element);
            }

            return root;
        }
    }
}
