//Assets\Vurbiri\Editor\Types\Collections\IdCollections\IdDrawer.cs
using System;
using System.Collections.Generic;
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
            Type t_attribute = typeof(NotIdAttribute), t_int = typeof(int);
            Type t_field = fieldInfo.FieldType;
            if (t_field.IsArray)
                t_field = t_field.GetElementType();

            t_field = t_field.GetGenericArguments()[0];

            FieldInfo[] fields = t_field.GetFields(BindingFlags.Public | BindingFlags.Static);

            int count = fields.Length;
            List<string> names = new(count);
            List<int> values = new(count);
            string name;
            int value;

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType != t_int || !field.IsLiteral || field.GetCustomAttributes(t_attribute, false).Length > 0)
                    continue;

                name = field.Name;
                value = (int)field.GetValue(null);
                name = $"{name} ({value})";

                names.Add(name);
                values.Add(value);
            }

            return (names.ToArray(), values.ToArray());
        }
    }
}
