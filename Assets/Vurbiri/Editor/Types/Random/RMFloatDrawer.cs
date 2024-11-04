using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(RMFloat))]
    public class RMFloatDrawer : ARValueDrawer
    {
        private const string NAME_VALUE = "_value";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty valueProperty = property.FindPropertyRelative(NAME_VALUE);
            valueProperty.floatValue = Mathf.Abs(valueProperty.floatValue);

            (Rect sizeLabel, Rect sizeMinus, Rect sizePlus) = CalkPosition(position);

            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(sizeLabel, label);
            valueProperty.floatValue = -EditorGUI.FloatField(sizeMinus, -valueProperty.floatValue);
            valueProperty.floatValue = EditorGUI.FloatField(sizePlus, valueProperty.floatValue);
            EditorGUI.EndProperty();

            valueProperty.floatValue = Mathf.Abs(valueProperty.floatValue);
        }
    }
}
