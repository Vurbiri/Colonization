using UnityEditor;
using UnityEngine;
using static Vurbiri.Colonization.UsedDebuffs;

namespace VurbiriEditor.Colonization
{
	public class UsedDebuffsDrawer : ASkillsDrawer
    {
        public UsedDebuffsDrawer(SerializedProperty parentProperty, int typeId, int id)
            : base(SkillType_Ed.Debuffs, parentProperty.FindPropertyRelative(arrayField), typeId, id, skillField) { }

        protected override void DrawListItems(Rect position, int index, bool isActive, bool isFocused)
        {
            var property = _list.serializedProperty.GetArrayElementAtIndex(index);
            position.height = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, _labels[property.FindPropertyRelative(skillField).intValue]))
            {
                position.y += _height; position.x += 15f; position.width -= 15f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative(chanceField));
            }
        }

        protected override float GetHeight(int index)
        {
            float height = _height;
            if (_list.serializedProperty.GetArrayElementAtIndex(index).isExpanded)
                height += _height;
            return height;
        }
    }
}
