//Assets\Vurbiri\Editor\Types\Random\RFloatDrawer.cs
using System.Reflection;
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

            MinMaxAttribute range = fieldInfo.GetCustomAttribute<MinMaxAttribute>();
            
            label = EditorGUI.BeginProperty(position, label, property);

            if (range != null)
            {
                var (sizeLabel, sizeMin, sizeSlider, sizeMax) = CalkPositionSlider(position);
                min = Mathf.Clamp(min, range.min, range.max);
                max = Mathf.Clamp(max, range.min, range.max);
                if (min > max) (min, max) = (max, min);

                EditorGUI.LabelField(sizeLabel, label);

                min = EditorGUI.FloatField(sizeMin, min);
                max = EditorGUI.FloatField(sizeMax, max);
                EditorGUI.MinMaxSlider(sizeSlider, ref min, ref max, range.min, range.max);
            }
            else
            {
                var (sizeLabel, sizeMin, sizeMax) = CalkPosition(position);
                EditorGUI.LabelField(sizeLabel, label);
                min = EditorGUI.FloatField(sizeMin, min);
                max = EditorGUI.FloatField(sizeMax, max);
            }

            EditorGUI.EndProperty();

            minProperty.floatValue = min; maxProperty.floatValue = max;
        }

    }
}

