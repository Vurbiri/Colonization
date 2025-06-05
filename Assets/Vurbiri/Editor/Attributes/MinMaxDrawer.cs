using System.Collections.Generic;
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

        private readonly HashSet<System.Type> _excludeTypes = new() { typeof(IntRnd), typeof(WaitRealtime), typeof(WaitTime) };

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
            if (attribute is not MinMaxAttribute range || _excludeTypes.Contains(fieldInfo.FieldType))
            {
                EditorGUI.PropertyField(position, mainProperty, label, true);
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
                EditorGUI.PropertyField(position, mainProperty, label, true);
                return;
            }

            label = EditorGUI.BeginProperty(position, label, mainProperty);

            if (minProperty.propertyType == SerializedPropertyType.Float)
            {
                VEditorGUI.MinMaxSlider(position, label, minProperty, maxProperty, range.min, range.max);

            }
            else if(minProperty.propertyType == SerializedPropertyType.Integer)
            {
                VEditorGUI.MinMaxSlider(position, label, minProperty, maxProperty, Mathf.RoundToInt(range.min), Mathf.RoundToInt(range.max));
            }
            else
            {
                EditorGUI.PropertyField(position, mainProperty, label, true);
            }
            EditorGUI.EndProperty();
        }
    }
}
