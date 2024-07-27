using UnityEditor;
using UnityEngine;

namespace Vurbiri
{
    [CustomPropertyDrawer(typeof(RMFloat))]
    public class RMFloatDrawer : PropertyDrawer
    {
        private const float OFFSET_SIZE_LABEL = 20f, SIZE_VALUE = 85f, SIZE_SPACE = 5f;
        private const string NAME_VALUE = "_value";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty valueProperty = property.FindPropertyRelative(NAME_VALUE);
            valueProperty.floatValue = Mathf.Abs(valueProperty.floatValue);

            Rect sizeLabel = position, sizeMinus = position, sizePlus = position;
            sizeLabel.width = EditorGUIUtility.labelWidth + OFFSET_SIZE_LABEL;
            sizeMinus.x = sizeLabel.width;
            sizePlus.x = sizeLabel.width + SIZE_VALUE + SIZE_SPACE;
            sizeMinus.width = sizePlus.width = SIZE_VALUE;

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(sizeLabel, label);
            valueProperty.floatValue = -EditorGUI.FloatField(sizeMinus, -valueProperty.floatValue);
            valueProperty.floatValue = EditorGUI.FloatField(sizePlus, valueProperty.floatValue);
            EditorGUI.EndProperty();

            valueProperty.floatValue = Mathf.Abs(valueProperty.floatValue);
        }
    }
}
