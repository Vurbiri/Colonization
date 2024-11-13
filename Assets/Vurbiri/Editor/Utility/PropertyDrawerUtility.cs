namespace VurbiriEditor
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using Vurbiri;

    public class PropertyDrawerUtility : PropertyDrawer
    {
        protected SerializedProperty _mainProperty;
        protected Rect _position;
        protected float _height, _ySpace;

        public override void OnGUI(Rect mainPosition, SerializedProperty mainProperty, GUIContent label)
        {
            _mainProperty = mainProperty;
            _ySpace = EditorGUIUtility.standardVerticalSpacing;
            _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            mainPosition.height = EditorGUIUtility.singleLineHeight;
            _position = mainPosition;
        }

        protected void Space(float ratio = 1f) => _position.y += _ySpace * ratio;

        protected void DrawBool(SerializedProperty parent, string name)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            property.boolValue = EditorGUI.Toggle(_position, property.displayName, property.boolValue);
        }
        protected void DrawBool(string name)
        {
            _position.y += _height;
            SerializedProperty property = _mainProperty.FindPropertyRelative(name);
            property.boolValue = EditorGUI.Toggle(_position, property.displayName, property.boolValue);
        }

        protected void DrawInt(SerializedProperty parent, string name)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            property.intValue = EditorGUI.IntField(_position, property.displayName, property.intValue);
        }
        protected void DrawInt(string name)
        {
            _position.y += _height;
            SerializedProperty property = _mainProperty.FindPropertyRelative(name);
            property.intValue = EditorGUI.IntField(_position, property.displayName, property.intValue);
        }

        protected void DrawIntSlider(SerializedProperty parent, string name, int min, int max)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            int value = Mathf.Clamp(property.intValue, min, max);
            property.intValue = EditorGUI.IntSlider(_position, property.displayName, value, min, max);
        }
        protected void DrawIntSlider(string name, int min, int max)
        {
            _position.y += _height;
            SerializedProperty property = _mainProperty.FindPropertyRelative(name);
            int value = Mathf.Clamp(property.intValue, min, max);
            property.intValue = EditorGUI.IntSlider(_position, property.displayName, value, min, max);
        }

        protected void DrawIntPopup(SerializedProperty parent, string name, string[] displayedOptions, int[] optionValues)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            property.intValue = EditorGUI.IntPopup(_position, property.displayName, property.intValue, displayedOptions, optionValues);
        }
        protected void DrawIntPopup(string name, string[] displayedOptions, int[] optionValues)
        {
            _position.y += _height;
            SerializedProperty property = _mainProperty.FindPropertyRelative(name);
            property.intValue = EditorGUI.IntPopup(_position, property.displayName, property.intValue, displayedOptions, optionValues);
        }

        protected void DrawPopup(SerializedProperty parent, string name, string[] displayedOptions)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            property.intValue = EditorGUI.Popup(_position, property.displayName, property.intValue, displayedOptions);
        }
        protected void DrawPopup(string name, string[] displayedOptions)
        {
            _position.y += _height;
            SerializedProperty property = _mainProperty.FindPropertyRelative(name);
            property.intValue = EditorGUI.Popup(_position, property.displayName, property.intValue, displayedOptions);
        }

        protected void DrawStringPopup(SerializedProperty parent, string name, string[] displayedOptions)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            int index = Array.IndexOf(displayedOptions, property.stringValue);
            property.stringValue = displayedOptions[EditorGUI.Popup(_position, property.displayName, index, displayedOptions)];
        }
        protected void DrawStringPopup(string name, string[] displayedOptions)
        {
            _position.y += _height;
            SerializedProperty property = _mainProperty.FindPropertyRelative(name);
            int index = Array.IndexOf(displayedOptions, property.stringValue);
            property.stringValue = displayedOptions[EditorGUI.Popup(_position, property.displayName, index, displayedOptions)];
        }

        protected void DrawId(SerializedProperty parent, string name, Type t_field, bool isNone = false)
        {
            var (names, values) = GetNamesAndValues(t_field, isNone);
            DrawIntPopup(parent, name, names, values);
        }
        protected void DrawId(string name, Type t_field, bool isNone = false)
        {
            var (names, values) = GetNamesAndValues(t_field, isNone);
            DrawIntPopup(_mainProperty, name, names, values);
        }

        protected SerializedProperty DrawObject<T>(SerializedProperty parent, string name, bool isName = false)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            if (isName)
            {
                property.objectReferenceValue = EditorGUI.ObjectField(_position, property.displayName, property.objectReferenceValue, typeof(T), false);
                return property;
            }
            property.objectReferenceValue = EditorGUI.ObjectField(_position, property.objectReferenceValue, typeof(T), false);
            return property;
        }
        protected SerializedProperty DrawObject<T>(string name, bool isName = false)
        {
            _position.y += _height;
            SerializedProperty property = _mainProperty.FindPropertyRelative(name);
            if (isName)
            {
                property.objectReferenceValue = EditorGUI.ObjectField(_position, property.displayName, property.objectReferenceValue, typeof(T), false);
                return property;
            }
            property.objectReferenceValue = EditorGUI.ObjectField(_position, property.objectReferenceValue, typeof(T), false);
            return property;
        }


        protected void DrawLine(Color color)
        {
            Rect size = _position;
            size.y += _height + _ySpace * 2f;
            size.x += 40;
            size.width -= 40;
            size.height = _ySpace;
            EditorGUI.DrawRect(size, color);
            _position.y += _ySpace * 5f;
        }

        protected List<string> GetNames(Type t_field)
        {
            Type t_attribute = typeof(NotIdAttribute), t_int = typeof(int);
            FieldInfo[] fields = t_field.GetFields(BindingFlags.Public | BindingFlags.Static);

            int count = fields.Length;
            List<string> strings = new(count);

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType != t_int || !field.IsLiteral || field.GetCustomAttributes(t_attribute, false).Length > 0 || (int)field.GetValue(null) < 0)
                    continue;

                strings.Add(field.Name);
            }

            return strings;
        }

        protected (string[] names, int[] values) GetNamesAndValues(Type t_field, bool isNone = false)
        {
            Type t_attribute = typeof(NotIdAttribute), t_int = typeof(int);

            FieldInfo[] fields = t_field.GetFields(BindingFlags.Public | BindingFlags.Static);

            int count = fields.Length + (isNone ? 1 : 0);
            List<string> names = new(count);
            List<int> values = new(count);

            if (isNone)
            {
                names.Add("None");
                values.Add(-1);
            }

            FieldInfo field;
            for (int i = 0; i < count; i++)
            {
                field = fields[i];

                if (field.FieldType != t_int | !field.IsLiteral || field.GetCustomAttributes(t_attribute, false).Length > 0)
                    continue;

                names.Add(field.Name);
                values.Add((int)field.GetValue(null));
            }

            return (names.ToArray(), values.ToArray());
        }
    }
}
