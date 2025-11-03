using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    public abstract class ActorsSettingsEditor<T> : AEditorGetVE<T> where T : ActorsSettingsEditor<T>
    {
        [SerializeField] private VisualTreeAsset _treeActorsSettingsScriptable;
        [SerializeField] private VisualTreeAsset _treeActorSettings;
        [SerializeField] private VisualTreeAsset _treeSkillsVT;

        private const string P_SETTINGS = "_settings", P_ARRAY = "_values", P_ID = "_id", P_SKILLS = "_skills";
        private const string U_CONTAINER = "Container", U_LABEL = "Label", U_SKILLS = "Skills";

        protected VisualElement CreateGUI<TId>(string captionText) where TId : ActorId<TId>
        {
            var root = _treeActorsSettingsScriptable.CloneTree();
            root.Q<Label>(U_LABEL).text = captionText;
            var container = root.Q<VisualElement>(U_CONTAINER);

            serializedObject.Update();
            var settingsProperty = serializedObject.FindProperty(P_SETTINGS).FindPropertyRelative(P_ARRAY);

            SerializedProperty sttProperty, skillsProperty;
            VisualElement element;
            for (int i = 0; i < IdType<TId>.Count; i++)
            {
                sttProperty = settingsProperty.GetArrayElementAtIndex(i);
                sttProperty.FindPropertyRelative(P_ID).intValue = i;
                skillsProperty = sttProperty.FindPropertyRelative(P_SKILLS);

                element = _treeActorSettings.Instantiate(sttProperty.propertyPath);
                element.Q<Foldout>(U_LABEL).text = IdType<TId>.Names_Ed[i];
                element.Q<VisualElement>(U_SKILLS).Add(_treeSkillsVT.Instantiate(skillsProperty.propertyPath));
                container.Add(element);
            }

            serializedObject.ApplyModifiedProperties();

            return root;
        }
    }
}
