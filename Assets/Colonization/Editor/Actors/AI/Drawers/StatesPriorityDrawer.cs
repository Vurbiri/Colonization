using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
	public class StatesPriorityDrawer<TId> where TId : ActorAIStateId<TId>
    {
        private const float WIDTH = 200f;

        private readonly SerializedProperty _arrayProperty;
        private readonly ReorderableList _list;
        private readonly GUIStyle _foldoutHeader;
        private readonly GUILayoutOption[] _options = { GUILayout.Width(WIDTH) , GUILayout.ExpandWidth(false) };

        public StatesPriorityDrawer(SerializedProperty mainProperty) 
		{
            _arrayProperty = mainProperty.FindPropertyRelative("_priority").FindPropertyRelative("_values");

            _list = new(mainProperty.serializedObject, _arrayProperty, true, false, false, false)
            {
                multiSelect = false,
                drawElementCallback = DrawListItems,
                drawElementBackgroundCallback = ReorderableListUtility.DrawBackground,
            };

            _foldoutHeader = new(EditorStyles.selectionRect)
            {
                name = "ListHeader",
                fixedWidth = WIDTH
            };
        }

        public void Draw()
        {
            Space();
            BeginVertical(STYLES.border);
            {
                BeginVertical(_options);
                Space(2f);
                if (_arrayProperty.isExpanded = BeginFoldoutHeaderGroup(_arrayProperty.isExpanded, "Priority", _foldoutHeader))
                {
                    _list.DoLayoutList();
                    Space(-_list.footerHeight);
                }
                EndFoldoutHeaderGroup();
                EndVertical();
                Space(2f);
            }
            EndVertical();
        }

        private void DrawListItems(Rect position, int index, bool isActive, bool isFocused)
        {
            position.x += 5f; position.width -= 5f;
            EditorGUI.LabelField(position, ActorAIStateId<TId>.Names_Ed[_list.serializedProperty.GetArrayElementAtIndex(index).intValue]);
        }
    }
}
