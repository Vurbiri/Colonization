//Assets\Vurbiri\Editor\Attributes\MinMaxDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(MinMaxAttribute))]
	public class MinMaxDrawer : ARValueDrawer
    {
        #region Consts
        private const string NAME_X = "x", NAME_Y = "y";
        #endregion

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
            if (attribute is not MinMaxAttribute range || fieldInfo.FieldType == typeof(IntRnd))
            {
                EditorGUILayout.PropertyField(mainProperty, label, true);
                return;
            }

            position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty minProperty, maxProperty;

            if (mainProperty.propertyType == SerializedPropertyType.Vector2 | mainProperty.propertyType == SerializedPropertyType.Vector2Int)
            {
                minProperty = mainProperty.FindPropertyRelative(NAME_X);
                maxProperty = mainProperty.FindPropertyRelative(NAME_Y);
            }
            else
            {
                minProperty = mainProperty.FindPropertyRelative(range.nameMin);
                maxProperty = mainProperty.FindPropertyRelative(range.nameMax);
            }

            if (minProperty == null | maxProperty == null || minProperty.propertyType != maxProperty.propertyType)
            {
                EditorGUILayout.PropertyField(mainProperty, label, true);
                return;
            }

            float min, max, rMin = range.min, rMax = range.max;

            if(minProperty.propertyType == SerializedPropertyType.Float)
            { 
                min = minProperty.floatValue;
                max = maxProperty.floatValue;
            }
            else if(minProperty.propertyType == SerializedPropertyType.Integer)
            { 
                min = minProperty.intValue;
                max = maxProperty.intValue;
                rMin = Mathf.Round(rMin);
                rMax = Mathf.Round(rMax);
            }
            else
            {
                EditorGUILayout.PropertyField(mainProperty, label, true);
                return;
            }

            min = Mathf.Clamp(min, rMin, rMax);
            max = Mathf.Clamp(max, rMin, rMax);

            var (sizeLabel, sizeMin, sizeSlider, sizeMax) = CalkPositionSlider(position);

            label = EditorGUI.BeginProperty(position, label, mainProperty);

            EditorGUI.LabelField(sizeLabel, label);

            min = EditorGUI.FloatField(sizeMin, min);
            max = EditorGUI.FloatField(sizeMax, max);
            EditorGUI.MinMaxSlider(sizeSlider, ref min, ref max, rMin, rMax);

            EditorGUI.EndProperty();

            if (min > max) (min, max) = (max, min);

            if (minProperty.propertyType == SerializedPropertyType.Float)
            { 
                minProperty.floatValue = min; 
                maxProperty.floatValue = max; 
            }
            else
            {
                minProperty.intValue = Mathf.RoundToInt(min);
                maxProperty.intValue = Mathf.RoundToInt(max);
            }
        }

	}
}
