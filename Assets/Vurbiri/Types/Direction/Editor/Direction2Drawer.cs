using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(Direction2))]
    public class Direction2Drawer : ADirectionDrawer<Vector2Int>
    {
        private static readonly string[] names = { "None", "Up", "Down", "Left", "Right" };
        private static readonly int[] indexes;
        private static readonly Vector2Int[] values = { Vector2Int.zero, Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};

        protected override string[] Names => names;
        protected override int[] Indexes => indexes;
        protected override Vector2Int[] Values => values;

        static Direction2Drawer()
        {
            indexes = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
                indexes[i] = i;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty vectorProperty = property.FindPropertyRelative(NAME_VALUE);

            vectorProperty.vector2IntValue = Drawer(position, property, label, vectorProperty.vector2IntValue);

        }
    }
}
