//Assets\Vurbiri\Editor\Types\Id\IdDrawer.cs
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(Id<>), true)]
    public class IdDrawer : PropertyDrawer
    {
        private const string NAME_VALUE = "_id";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string[] names = null; int[] values = null;
            if(!GetNamesAndValues(ref names, ref values))
            {
                EditorGUI.HelpBox(position, $"Error getting values", UnityEditor.MessageType.Error);
                return;
            }

            SerializedProperty valueProperty = property.FindPropertyRelative(NAME_VALUE);
            label = EditorGUI.BeginProperty(position, label, property);
            {
                valueProperty.intValue = EditorGUI.IntPopup(position, label.text, valueProperty.intValue, names, values, EditorStyles.popup);
            }
            EditorGUI.EndProperty();
        }

        private bool GetNamesAndValues(ref string[] names, ref int[] values)
        {
            Type typeField = fieldInfo.FieldType;
            if (typeField.IsArray) typeField = typeField.GetElementType();

            Type[] arg = typeField.GetGenericArguments();
            if (arg == null || arg.Length != 1) return false;
            typeField = typeField.GetGenericArguments()[0].BaseType;

            PropertyInfo displayNamesProperty = null, valuesProperty = null;
            while (typeField != null & (displayNamesProperty == null | valuesProperty == null))
            {
                 displayNamesProperty = typeField.GetProperty("DisplayNames");
                valuesProperty = typeField.GetProperty("Values");
                typeField = typeField.BaseType;
            }

            if (displayNamesProperty == null | valuesProperty == null) return false;

            names = (string[])displayNamesProperty.GetValue(null);
            values = (int[])valuesProperty.GetValue(null);

            return names != null & values != null;
        }
    }
}
