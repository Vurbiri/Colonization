//Assets\Vurbiri\Editor\Types\Random\RIntDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(RInt))]
    public class RIntDrawer : ARValueDrawer
    {
        private const string NAME_MIN = "_min", NAME_MAX = "_max";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty minProperty = property.FindPropertyRelative(NAME_MIN);
            SerializedProperty maxProperty = property.FindPropertyRelative(NAME_MAX);

            (Rect sizeLabel, Rect sizeMin, Rect sizeMax) = CalkPosition(position);

            label = EditorGUI.BeginProperty(position, label, property);

            EditorGUI.LabelField(sizeLabel, label);
            minProperty.intValue = EditorGUI.IntField(sizeMin, minProperty.intValue);
            maxProperty.intValue = EditorGUI.IntField(sizeMax, maxProperty.intValue - 1) + 1;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;
    }
}
