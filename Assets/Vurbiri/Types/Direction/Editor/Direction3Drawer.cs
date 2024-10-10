using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(Direction3))]
    public class Direction3Drawer : ADirectionDrawer<Vector3Int>
    {
        private static readonly string[] names = { "None", "Up", "Down", "Left", "Right", "Forward", "Back" };
        private static readonly int[] indexes;
        private static readonly Vector3Int[] values = { Vector3Int.zero, Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.back };

        protected override string[] Names => names;
        protected override int[] Indexes => indexes;
        protected override Vector3Int[] Values => values;

        static Direction3Drawer()
        {
            indexes = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
                indexes[i] = i;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty vectorProperty = property.FindPropertyRelative(NAME_VALUE);

            vectorProperty.vector3IntValue = Drawer(position, property, label, vectorProperty.vector3IntValue);

        }
    }
}
