using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization
{
	[CustomPropertyDrawer(typeof(SkillApplied))]
	public class SkillAppliedDrawer : PropertyDrawer
	{
        private const float ANTY_SIZE = 112f, AP_SIZE = 77, APPLY_SIZE = 62f;
        private static readonly GUIContent _appliedTitle = new("Applied?");

		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
            float indent = indentLevel * 15f;
            position.height = EditorGUIUtility.singleLineHeight;
            position.x += (position.width - (ANTY_SIZE + AP_SIZE + APPLY_SIZE * 3f)) * 0.5f;

            label = BeginProperty(position, _appliedTitle, mainProperty);
            {
                position.width = ANTY_SIZE + indent;
                ToggleDraw(position, mainProperty.FindPropertyRelative(SkillApplied.antipodeField));

                position.width = AP_SIZE + indent; position.x += ANTY_SIZE;
                ToggleDraw(position, mainProperty.FindPropertyRelative(SkillApplied.maxAPField));

                position.width = APPLY_SIZE + indent; position.x += AP_SIZE;
                LabelField(position, label);
                position.x += APPLY_SIZE;
                ToggleDraw(position, mainProperty.FindPropertyRelative(SkillApplied.selfField));
                position.x += APPLY_SIZE;
                ToggleDraw(position, mainProperty.FindPropertyRelative(SkillApplied.targetField));
            }
            EndProperty();

            static void ToggleDraw(Rect position, SerializedProperty property)
            {
                property.boolValue = ToggleLeft(position, property.displayName, property.boolValue);
            }
        }
    }
}