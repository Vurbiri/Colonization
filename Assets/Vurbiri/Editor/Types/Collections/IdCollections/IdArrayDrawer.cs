using System;
using UnityEditor;
using UnityEngine;
using Vurbiri.Collections;

namespace VurbiriEditor.Collections
{
    [CustomPropertyDrawer(typeof(IdArray<,>))]
    sealed public class IdArrayDrawer : AIdCollectionDrawer
    {
        private Func<Rect, SerializedProperty, string, Rect> _propertyField;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            
            label = EditorGUI.BeginProperty(position, label, property);
            {
                if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label) && property.hasChildren)
                {
                    SerializedProperty propertyValues = property.FindPropertyRelative(NAME_ARRAY);

                    if (!SetPositiveNames() || _propertyField == null)
                    {
                        if (Utility.IsUnityProperty(propertyValues.GetArrayElementAtIndex(0)) || Utility.IsCustomProperty(fieldInfo.FieldType.GetGenericArguments()[INDEX_VALUE]))
                            _propertyField = VEditorGUI.CustomPropertyField;
                        else
                            _propertyField = VEditorGUI.DefaultPropertyField;
                    }

                    propertyValues.arraySize = _count;

                    position.y += _height;
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < _count; i++)
                        position = _propertyField(position, propertyValues.GetArrayElementAtIndex(i), _names[i]);
                    EditorGUI.indentLevel--;
                }
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
    }
}
