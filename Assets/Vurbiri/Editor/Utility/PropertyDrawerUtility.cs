using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    public class PropertyDrawerUtility : PropertyDrawer
    {
        protected SerializedProperty _mainProperty;
        protected Rect _position;
        protected float _height, _ySpace;

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            _mainProperty = mainProperty;
            _ySpace = EditorGUIUtility.standardVerticalSpacing;
            _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            position.height = EditorGUIUtility.singleLineHeight;
            _position = position;
        }

        protected void Space(float ratio = 1f) => _position.y += _ySpace * ratio;

        protected bool Foldout(GUIContent label) => _mainProperty.isExpanded = EditorGUI.Foldout(_position, _mainProperty.isExpanded, label);

        protected bool DrawBool(SerializedProperty parent, string name)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            return property.boolValue = EditorGUI.Toggle(_position, property.displayName, property.boolValue);
        }
        protected bool DrawBool(string name) => DrawBool(_mainProperty, name);

        protected int DrawInt(SerializedProperty parent, string name)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            return property.intValue = EditorGUI.IntField(_position, property.displayName, property.intValue);
        }
        protected int DrawInt(string name) => DrawInt(_mainProperty, name);

        protected int DrawIntSlider(SerializedProperty parent, string name, int min, int max)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            int value = Mathf.Clamp(property.intValue, min, max);
            return property.intValue = EditorGUI.IntSlider(_position, property.displayName, value, min, max);
        }
        protected int DrawIntSlider(string name, int min, int max) => DrawIntSlider(_mainProperty, name, min, max);

        protected int DrawIntPopup(SerializedProperty parent, string name, string[] displayedOptions, int[] optionValues)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            return property.intValue = EditorGUI.IntPopup(_position, property.displayName, property.intValue, displayedOptions, optionValues);
        }
        protected int DrawIntPopup(string name, string[] displayedOptions, int[] optionValues) => DrawIntPopup(_mainProperty, name, displayedOptions, optionValues);

        protected int DrawPopup(SerializedProperty parent, string name, string[] displayedOptions)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            return property.intValue = EditorGUI.Popup(_position, property.displayName, property.intValue, displayedOptions);
        }
        protected int DrawPopup(string name, string[] displayedOptions) => DrawPopup(_mainProperty, name, displayedOptions);

        protected String DrawStringPopup(SerializedProperty parent, string name, string[] displayedOptions)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            int index = Array.IndexOf(displayedOptions, property.stringValue);
            index = EditorGUI.Popup(_position, property.displayName, index, displayedOptions);
            return property.stringValue = displayedOptions[index];
        }
        protected String DrawStringPopup(string name, string[] displayedOptions) => DrawStringPopup(_mainProperty, name, displayedOptions);

        protected int DrawId(SerializedProperty parent, string name, Type t_field, bool isNone = false)
        {
            var (names, values) = GetNamesAndValues(t_field, isNone);
            return DrawIntPopup(parent, name, names, values);
        }
        protected int DrawId(string name, Type t_field, bool isNone = false) => DrawId(_mainProperty, name, t_field, isNone);

        protected T DrawObject<T>(SerializedProperty parent, string name, bool isName = false) where T : UnityEngine.Object
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);
            if (isName)
                return (T)(property.objectReferenceValue = EditorGUI.ObjectField(_position, property.displayName, property.objectReferenceValue, typeof(T), false));
            return (T)(property.objectReferenceValue = EditorGUI.ObjectField(_position, property.objectReferenceValue, typeof(T), false));
        }
        protected T DrawObject<T>(string name, bool isName = false) where T : UnityEngine.Object => DrawObject<T>(_mainProperty, name, isName);

        protected void DrawLabel(string name, string value)
        {
            _position.y += _height;
            EditorGUI.LabelField(_position, name, value);
        }

        protected void DrawLabelAndSetValue<T>(SerializedProperty parent, string name, T value)
        {
            _position.y += _height;
            SerializedProperty property = parent.FindPropertyRelative(name);

            if (value is float fValue)
                property.floatValue = fValue;
            else if (value is int iValue)
                property.intValue = iValue;
            else if (value is bool bValue)
                property.boolValue = bValue;

            EditorGUI.LabelField(_position, $"{property.displayName}", $"{value}");
        }
        protected void DrawLabelAndSetValue<T>(string name, T value) => DrawLabelAndSetValue<T>(_mainProperty, name, value);

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

            int count = fields.Length;
            List<string> names = new(count + 1);
            List<int> values = new(count + 1);

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

        protected int IdFromLabel(GUIContent label)
        {
            string[] strings = label.text.Split(' ');
            int id = -1;
            if (strings.Length == 2)
                id = int.Parse(strings[1]);

            return id;
        }
    }
}
