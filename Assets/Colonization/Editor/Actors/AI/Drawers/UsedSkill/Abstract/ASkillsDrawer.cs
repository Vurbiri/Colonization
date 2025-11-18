using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
	public abstract class ASkillsDrawer
	{
        private readonly bool _isDraw;
        private readonly GUIContent _name;

        protected readonly GUIContent[] _labels;
        protected readonly ReorderableList _list;
        protected readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        protected ASkillsDrawer(Id<SkillType_Ed> skill, SerializedProperty skills, int typeId, int id, string skillFieldName)
		{
            var values = SkillDrawer.GetValues(skill, typeId, id);
            int count = values.Length;

            _isDraw = count > 0;

            if (_isDraw)
            {
                if(!SkillDrawer.OnValidate(skills, values, skillFieldName))
                {
                    for (int i = 0; i < count; ++i)
                        SetDefault(skills.GetArrayElementAtIndex(i), typeId, id, i);
                }

                _labels = SkillDrawer.GetLabels(skill, typeId, id);
                _list = new(skills.serializedObject, skills, true, false, false, false)
                {
                    multiSelect = false,
                    drawElementCallback = DrawListItems,
                    drawElementBackgroundCallback = ReorderableListUtility.DrawBackground,
                    elementHeightCallback = GetHeight
                };

                _name = new(SkillType_Ed.Names_Ed[skill]);
            }
        }

        public void Draw()
        {
            if (_isDraw)
            {
                if (_list.serializedProperty.isExpanded = Foldout(_list.serializedProperty.isExpanded, _name))
                {
                    _list.DoLayoutList();
                    Space(-_list.footerHeight);
                }
                Space();
            }
        }

        protected virtual void SetDefault(SerializedProperty skill, int typeId, int id, int index) { }

        protected abstract void DrawListItems(Rect position, int index, bool isActive, bool isFocused);
        protected abstract float GetHeight(int index);
    }
}
