//Assets\Vurbiri\Editor\Types\Random\MinMaxDrawer.cs
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
            if (attribute is not MinMaxAttribute range) return;
            
            if(mainProperty.propertyType != SerializedPropertyType.Vector2 & mainProperty.propertyType != SerializedPropertyType.Vector2Int)
            {
                EditorGUI.PropertyField(position, mainProperty, label);
                //EditorGUILayout.PropertyField(mainProperty);
                return;
            }
            
            position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty xProperty = mainProperty.FindPropertyRelative(NAME_X);
            SerializedProperty yProperty = mainProperty.FindPropertyRelative(NAME_Y);

            float min, max, rMin = range.min, rMax = range.max;

            if(mainProperty.propertyType == SerializedPropertyType.Vector2)
            { 
                min = xProperty.floatValue;
                max = yProperty.floatValue;
            }
            else
            { 
                min = xProperty.intValue;
                max = yProperty.intValue;
                rMin = Mathf.Round(rMin);
                rMax = Mathf.Round(rMax);
            }

            min = Mathf.Clamp(min, rMin, rMax);
            max = Mathf.Clamp(max, rMin, rMax);
            if (min > max) (min, max) = (max, min);

            label = EditorGUI.BeginProperty(position, label, mainProperty);

            var (sizeLabel, sizeMin, sizeSlider, sizeMax) = CalkPositionSlider(position);

            EditorGUI.LabelField(sizeLabel, label);

            min = EditorGUI.FloatField(sizeMin, min);
            max = EditorGUI.FloatField(sizeMax, max);
            EditorGUI.MinMaxSlider(sizeSlider, ref min, ref max, rMin, rMax);

            EditorGUI.EndProperty();

            if (mainProperty.propertyType == SerializedPropertyType.Vector2)
            { 
                xProperty.floatValue = min; 
                yProperty.floatValue = max; 
            }
            else
            {
                xProperty.intValue = Mathf.RoundToInt(min);
                yProperty.intValue = Mathf.RoundToInt(max);
            }

        }

	}
}