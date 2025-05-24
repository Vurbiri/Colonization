using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(Direction2))]
    public class Direction2Drawer : ADirectionDrawer<Vector2Int>
    {
        private static readonly string[] s_names = { "None", "Up", "Down", "Left", "Right" };
        private static readonly int[] s_indexes;
        private static readonly Vector2Int[] s_values = { Vector2Int.zero, Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};

        protected override string[] Names => s_names;
        protected override int[] Indexes => s_indexes;
        protected override Vector2Int[] Values => s_values;

        static Direction2Drawer()
        {
            s_indexes = new int[s_values.Length];
            for (int i = 0; i < s_values.Length; i++)
                s_indexes[i] = i;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty vectorProperty = property.FindPropertyRelative(NAME_VALUE);

            vectorProperty.vector2IntValue = Drawer(position, property, label, vectorProperty.vector2IntValue);

        }
    }
}
