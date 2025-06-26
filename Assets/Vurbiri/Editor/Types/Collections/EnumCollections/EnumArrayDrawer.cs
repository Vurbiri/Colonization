using System;
using UnityEditor;
using UnityEngine;
using Vurbiri.Collections;

namespace VurbiriEditor.Collections
{
    [CustomPropertyDrawer(typeof(EnumArray<,>))]
    sealed public class EnumArrayDrawer : PropertyDrawer
    {
        private readonly int INDEX_TYPE = 0, INDEX_VALUE = 1;
        private readonly string NAME_ARRAY = "_values";

        private Type _enumType;
        private string[] _names;
        private Func<Rect, SerializedProperty, string, Rect> _propertyField;

        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        private readonly float _ySpace = EditorGUIUtility.standardVerticalSpacing;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            label = EditorGUI.BeginProperty(position, label, property);

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label) && property.hasChildren)
            {
                SerializedProperty propertyValues = property.FindPropertyRelative(NAME_ARRAY);
                int count = propertyValues.arraySize;

                SetNamesAndDrawer(propertyValues);

                position.y += _height;
                EditorGUI.indentLevel++;
                for (int i = 0; i < count; i++)
                    position = _propertyField(position, propertyValues.GetArrayElementAtIndex(i), _names[i]);
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = _height;

            if (property.isExpanded && property.hasChildren)
            {
                SerializedProperty propertyValues = property.FindPropertyRelative(NAME_ARRAY);
                int count = propertyValues.arraySize;
                for (int i = 0; i < count; i++)
                    height += EditorGUI.GetPropertyHeight(propertyValues.GetArrayElementAtIndex(i)) + _ySpace;
            }

            return height;
        }

        private void SetNamesAndDrawer(SerializedProperty property)
        {
            Type enumType = fieldInfo.FieldType.GetGenericArguments()[INDEX_TYPE];
            bool isNewType = enumType != _enumType;

            if (isNewType | _names == null) 
                _names = Enum.GetNames(enumType);

            if (isNewType | _propertyField == null)
            {
                if (Utility.IsUnityProperty(property.GetArrayElementAtIndex(0)) || Utility.IsCustomProperty(fieldInfo.FieldType.GetGenericArguments()[INDEX_VALUE]))
                    _propertyField = VEditorGUI.CustomPropertyField;
                else
                    _propertyField = VEditorGUI.DefaultPropertyField;
            }

            _enumType = enumType;
        }
    }
}
