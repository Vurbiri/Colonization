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

            float min = minProperty.floatValue, max = maxProperty.floatValue;
            if (min > max) (min, max) = (max, min);
            
            label = EditorGUI.BeginProperty(position, label, property);

            var (sizeLabel, sizeMin, sizeMax) = CalkPosition(position);
            EditorGUI.LabelField(sizeLabel, label);
            minProperty.floatValue = EditorGUI.FloatField(sizeMin, min);
            maxProperty.floatValue = EditorGUI.FloatField(sizeMax, max);

            EditorGUI.EndProperty();
        }

    }
}

