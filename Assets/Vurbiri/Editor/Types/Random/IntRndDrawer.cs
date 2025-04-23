//Assets\Vurbiri\Editor\Types\Random\RIntDrawer.cs
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(IntRnd))]
    public class IntRndDrawer : ARValueDrawer
    {
        private const string NAME_MIN = "_min", NAME_MAX = "_max";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty minProperty = property.FindPropertyRelative(NAME_MIN);
            SerializedProperty maxProperty = property.FindPropertyRelative(NAME_MAX);

            MinMaxAttribute range = fieldInfo.GetCustomAttribute<MinMaxAttribute>();

            label = EditorGUI.BeginProperty(position, label, property);

            if (range != null)
            {
                var (sizeLabel, sizeMin, sizeSlider, sizeMax) = CalkPositionSlider(position);
                float min, max, rMin = Mathf.Round(range.min), rMax = Mathf.Round(range.max);
                min = Mathf.Clamp(minProperty.intValue, rMin, rMax);
                max = Mathf.Clamp(maxProperty.intValue - 1, rMin, rMax);

                EditorGUI.LabelField(sizeLabel, label);

                min = EditorGUI.FloatField(sizeMin, min);
                max = EditorGUI.FloatField(sizeMax, max);

                EditorGUI.MinMaxSlider(sizeSlider, ref min, ref max, rMin, rMax);

                if (min > max) (min, max) = (max, min);
                minProperty.intValue = Mathf.RoundToInt(min); 
                maxProperty.intValue = Mathf.RoundToInt(max) + 1;
            }
            else
            {
                int min = minProperty.intValue, max = maxProperty.intValue - 1;
                
                var (sizeLabel, sizeMin, sizeMax) = CalkPosition(position);
                EditorGUI.LabelField(sizeLabel, label);
                min = EditorGUI.IntField(sizeMin, min);
                max = EditorGUI.IntField(sizeMax, max);

                if (min > max) (min, max) = (max, min);
                minProperty.intValue = min;
                maxProperty.intValue = max + 1;
            }

            EditorGUI.EndProperty();
        }
    }
}
