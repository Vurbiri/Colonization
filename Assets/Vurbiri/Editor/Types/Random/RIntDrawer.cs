//Assets\Vurbiri\Editor\Types\Random\RIntDrawer.cs
using System.Reflection;
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

            MinMaxAttribute range = fieldInfo.GetCustomAttribute<MinMaxAttribute>();

            label = EditorGUI.BeginProperty(position, label, property);

            if (range != null)
            {
                var (sizeLabel, sizeMin, sizeSlider, sizeMax) = CalkPositionSlider(position);
                float min, max, rMin = Mathf.Round(range.min), rMax = Mathf.Round(range.max);

                EditorGUI.LabelField(sizeLabel, label);

                min = EditorGUI.IntField(sizeMin, minProperty.intValue);
                max = EditorGUI.IntField(sizeMax, maxProperty.intValue - 1);

                min = Mathf.Clamp(min, rMin, rMax);
                max = Mathf.Clamp(max, rMin, rMax);
                if (min > max) (min, max) = (max, min);

                EditorGUI.MinMaxSlider(sizeSlider, ref min, ref max, rMin, rMax);

                minProperty.intValue = Mathf.RoundToInt(min); 
                maxProperty.intValue = Mathf.RoundToInt(max) + 1;
            }
            else
            {
                var (sizeLabel, sizeMin, sizeMax) = CalkPosition(position);
                EditorGUI.LabelField(sizeLabel, label);
                minProperty.intValue = EditorGUI.IntField(sizeMin, minProperty.intValue);
                maxProperty.intValue = EditorGUI.IntField(sizeMax, maxProperty.intValue - 1) + 1;
            }

            EditorGUI.EndProperty();
        }

    }
}
