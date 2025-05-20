//Assets\Vurbiri\Editor\Random\FloatZRndDrawer.cs
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(FloatZRnd))]
    public class FloatZRndDrawer : ARValueDrawer
    {
        private const string NAME_VALUE = "_value";
        private const float ZERO = 0.00001f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty valueProperty = property.FindPropertyRelative(NAME_VALUE);
            float max, min;
            float value = valueProperty.floatValue;

            var range = fieldInfo.GetCustomAttribute<MaxAttribute>();

            label = EditorGUI.BeginProperty(position, label, property);

            if (range != null)
            {
                if(range.max >= 0) { max = range.max; min = ZERO; }
                else { min = range.max; max = -ZERO; }

                value = EditorGUI.Slider(position, label, value, min, max);
            }
            else
            {
                var (sizeLabel, sizeMin, sizeMax) = CalkPosition(position);
                EditorGUI.LabelField(sizeLabel, label);
                if (value >= 0f) 
                {
                    EditorGUI.FloatField(sizeMin, 0f);
                    value = EditorGUI.FloatField(sizeMax, value);
                }
                else 
                {
                    value = EditorGUI.FloatField(sizeMin, value);
                    EditorGUI.FloatField(sizeMax, 0f);
                }
            }

            EditorGUI.EndProperty();

            valueProperty.floatValue = value;
        }
    }
}
