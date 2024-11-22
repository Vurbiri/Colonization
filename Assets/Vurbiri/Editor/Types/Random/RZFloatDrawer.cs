//Assets\Vurbiri\Editor\Types\Random\RZFloatDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(RZFloat))]
    public class RZFloatDrawer : PropertyDrawer
    {
        private const float OFFSET_SIZE_LABEL = 20f, SIZE_VALUE = 85f, SIZE_SPACE = 5f;
        private const string NAME_VALUE = "_value";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty valueProperty = property.FindPropertyRelative(NAME_VALUE);

            Rect sizeLabel = position, sizeZero = position, sizeValue = position;

            sizeLabel.width = EditorGUIUtility.labelWidth + OFFSET_SIZE_LABEL;

            if (valueProperty.floatValue >= 0f)
            {
                sizeZero.x = sizeLabel.width;
                sizeValue.x = sizeLabel.width + SIZE_VALUE + SIZE_SPACE;
            }
            else
            {
                sizeZero.x = sizeLabel.width + SIZE_VALUE + SIZE_SPACE;
                sizeValue.x = sizeLabel.width;
            }

            sizeZero.width = sizeValue.width = SIZE_VALUE;

            label = EditorGUI.BeginProperty(position, label, property);

            EditorGUI.LabelField(sizeLabel, label);
            EditorGUI.FloatField(sizeZero, 0f);
            valueProperty.floatValue = EditorGUI.FloatField(sizeValue, valueProperty.floatValue);

            EditorGUI.EndProperty();
        }
    }
}
