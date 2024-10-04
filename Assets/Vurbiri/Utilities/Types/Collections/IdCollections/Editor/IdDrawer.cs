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

            var v = GetNamesAndValues();

            label = EditorGUI.BeginProperty(position, label, property);
            valueProperty.intValue = EditorGUI.IntPopup(position, label.text, valueProperty.intValue, v.strings, v.ints, EditorStyles.popup);
            EditorGUI.EndProperty();

            #region Local GetNamesAndValues()
            //=====================================================
            (string[] strings, int[] ints) GetNamesAndValues()
            {
                Type t_attribute = typeof(NotIdAttribute), t_int = typeof(int);
                FieldInfo[] fields = fieldInfo.FieldType.GetGenericArguments()[0].GetFields(BindingFlags.Public | BindingFlags.Static);
                
                int count = fields.Length;
                List<string> strings = new(count);
                List<int> ints = new(count);

                foreach (FieldInfo field in fields)
                {
                    if (field.GetCustomAttributes(t_attribute, false).Length > 0 || field.FieldType != t_int || !field.IsLiteral)
                        continue;

                    strings.Add(field.Name);
                    ints.Add((int)field.GetValue(null));
                }

                return (strings.ToArray(), ints.ToArray());
            }
            #endregion 
        }
    }
}
