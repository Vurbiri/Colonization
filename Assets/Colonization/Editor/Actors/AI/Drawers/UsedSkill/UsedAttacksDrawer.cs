using UnityEditor;
using UnityEngine;
using static Vurbiri.Colonization.UsedAttacks;

namespace VurbiriEditor.Colonization
{
	public class UsedAttacksDrawer : ASkillsDrawer
    {
        public UsedAttacksDrawer(SerializedProperty parentProperty, int typeId, int id)
            : base(SkillType_Ed.Attacks, parentProperty.FindPropertyRelative(arrayField), typeId, id, skillField) { }

        protected override void DrawListItems(Rect position, int index, bool isActive, bool isFocused)
        {
            var property = _list.serializedProperty.GetArrayElementAtIndex(index);
            position.height = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, _labels[property.FindPropertyRelative(skillField).intValue]))
            {
                position.y += _height + 2f; position.x += 15f; position.width -= 15f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative(appliedField));
                position.y += _height + 4f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative(chanceField));
                position.y += _height + 4f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative(selfHPField));
                position.y += _height;
                EditorGUI.PropertyField(position, property.FindPropertyRelative(targetHPField));
            }
        }

        protected override float GetHeight(int index)
        {
            float height = _height;
            if (_list.serializedProperty.GetArrayElementAtIndex(index).isExpanded)
                height += _height * 4f + 12f;
            return height;
        }
    }
}
