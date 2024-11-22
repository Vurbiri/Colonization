//Assets\Vurbiri\Editor\Types\Random\RFloatDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(RFloat))]
    public class RFloatDrawer : ARValueDrawer
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
            minProperty.floatValue = EditorGUI.FloatField(sizeMin, minProperty.floatValue);
            maxProperty.floatValue = EditorGUI.FloatField(sizeMax, maxProperty.floatValue);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;
    }
}

