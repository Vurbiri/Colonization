using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
	public class UsedSelfSkillsDrawer
	{
        private readonly int _typeId, _id;
        private readonly bool _isDraw;

        private readonly ReorderableList _list;
        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public UsedSelfSkillsDrawer(SerializedProperty parentProperty, int typeId, int id)
        {
            int count = SkillDrawer.SelfCount(typeId, id);
            _isDraw = count > 0;
            if (_isDraw)
            {
                var skills = parentProperty.FindPropertyRelative("_skills");
                Create(skills, typeId, id);

                _list = new(parentProperty.serializedObject, skills, true, false, false, false)
                {
                    multiSelect = false,
                    drawElementCallback = DrawListItems,
                    drawElementBackgroundCallback = ReorderableListUtility.DrawBackground,
                    elementHeightCallback = GetHeight
                };
            }
        }

        public void Draw()
        {
            if(_isDraw)
            {
                if (_list.serializedProperty.isExpanded = Foldout(_list.serializedProperty.isExpanded, "Self Buffs"))
                {
                    _list.DoLayoutList();
                    Space(-_list.footerHeight);
                }
                Space();
            }
        }

        private void DrawListItems(Rect position, int index, bool isActive, bool isFocused)
        {
            var property = _list.serializedProperty.GetArrayElementAtIndex(index);
            GUIContent label = new (SkillDrawer.GetSelfName(_typeId, _id, property.FindPropertyRelative("skill").intValue));
            position.height = EditorGUIUtility.singleLineHeight;

            if(property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
            {
                position.y += _height; position.x += 15f; position.width -= 15f;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("chance"));
                position.y += _height;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("currentHP"));
            }
        }

        private float GetHeight(int index)
        {
            float height = _height;
            if (_list.serializedProperty.GetArrayElementAtIndex(index).isExpanded)
                height += _height * 2f;
            return height;
        }

        private static void Create(SerializedProperty skills, int typeId, int id)
        {
            int count = SkillDrawer.SelfCount(typeId, id);
            if (skills.arraySize != count)
            {
                var values = SkillDrawer.GetSelfValues(typeId, id);
                
                skills.arraySize = count;
                for(int i = 0; i < count; ++i)
                    skills.GetArrayElementAtIndex(i).FindPropertyRelative("skill").intValue = values[i];
                
                skills.serializedObject.ApplyModifiedProperties();
            }
        }

        private class SelfSkillDrawer
        {
            private readonly SerializedProperty _parentProperty;
            private readonly SerializedProperty _skillProperty;
            private readonly SerializedProperty _currentHPProperty;
            private readonly SerializedProperty _chanceProperty;
            private readonly GUIContent _label = new(" └─ Chance");
        }
    }
}
