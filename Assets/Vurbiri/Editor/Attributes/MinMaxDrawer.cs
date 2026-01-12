using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(MinMaxAttribute))]
	public class MinMaxDrawer : ARValueDrawer
    {
        private readonly List<System.Type> _excludeTypes = new() { typeof(IntRnd), typeof(WaitRealtime), typeof(WaitScaledTime) };
        private readonly System.Type _refValue = typeof(Ref<>);

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
            if (attribute is not MinMaxAttribute range || _excludeTypes.Contains(fieldInfo.FieldType))
            {
                PropertyField(position, mainProperty, label, true);
                return;
            }

            if(fieldInfo.FieldType.IsGeneric(_refValue))
            {
                DrawRefValue(position, mainProperty.FindPropertyRelative("_value"), label, range);
                return;
            }

            SerializedProperty minProperty, maxProperty;

            if (mainProperty.propertyType == SerializedPropertyType.Vector2 | mainProperty.propertyType == SerializedPropertyType.Vector2Int)
            {
                minProperty = mainProperty.FindPropertyRelative(nameof(Vector2.x));
                maxProperty = mainProperty.FindPropertyRelative(nameof(Vector2.y));
            }
            else
            {
                minProperty = mainProperty.FindPropertyRelative(range.nameMin);
                maxProperty = mainProperty.FindPropertyRelative(range.nameMax);
            }

            if (minProperty == null || maxProperty == null || minProperty.propertyType != maxProperty.propertyType)
            {
                PropertyField(position, mainProperty, label, true);
                return;
            }

            position.height = EditorGUIUtility.singleLineHeight;

            label = BeginProperty(position, label, mainProperty);
            {
                if (minProperty.propertyType == SerializedPropertyType.Float)
                {
                    VEditorGUI.MinMaxSlider(position, label, minProperty, maxProperty, range.min, range.max);

                }
                else if (minProperty.propertyType == SerializedPropertyType.Integer)
                {
                    VEditorGUI.MinMaxSlider(position, label, minProperty, maxProperty, range.min.Round(), range.max.Round());
                }
                else
                {
                    PropertyField(position, mainProperty, label, true);
                }
            }
            EndProperty();
        }

        private static void DrawRefValue(Rect position, SerializedProperty property, GUIContent label, MinMaxAttribute range)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            label = BeginProperty(position, label, property);
            {
                if (property.propertyType == SerializedPropertyType.Float)
                {
                    property.floatValue = Slider(position, label, property.floatValue, range.min, range.max);
                }
                else if (property.propertyType == SerializedPropertyType.Integer)
                {
                    property.intValue = IntSlider(position, label, property.intValue, range.min.Round(), range.max.Round());
                }
                else
                {
                    PropertyField(position, property, label);
                }
            }
            EndProperty();
        }
    }
}
