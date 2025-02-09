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
            SerializedProperty valueProperty = property.FindPropertyRelative(NAME_VALUE);
            var (names, values) = GetNamesAndValues();

            label = EditorGUI.BeginProperty(position, label, property);
            valueProperty.intValue = EditorGUI.IntPopup(position, label.text, valueProperty.intValue, names, values, EditorStyles.popup);
            EditorGUI.EndProperty();
        }

        private (string[] names, int[] values) GetNamesAndValues()
        {
            Type typeField = fieldInfo.FieldType;
            PropertyInfo _displayNames, _values;

            if (typeField.IsArray)
                typeField = typeField.GetElementType();

            typeField = typeField.GetGenericArguments()[0];

            do
            {
                typeField = typeField.BaseType;
                _displayNames = typeField.GetProperty("DisplayNames");
                _values = typeField.GetProperty("Values");
            }
            while (_displayNames == null | _values == null);

            return ((string[])_displayNames.GetValue(null), (int[])_values.GetValue(null));
        }
    }
}
