//Assets\Vurbiri\Editor\Utility\PropertyDrawerUtility.cs
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

        //================================================================
        #region DrawBool
        protected bool DrawBool(SerializedProperty property, bool isName = true)
        {
            if (isName) return DrawBool(property, property.displayName);

            _position.y += _height;
            return property.boolValue = EditorGUI.Toggle(_position, property.boolValue);
        }
        protected bool DrawBool(SerializedProperty property, string displayName)
        {
            _position.y += _height;
            return property.boolValue = EditorGUI.Toggle(_position, displayName, property.boolValue);
        }
        protected bool DrawBool(string nameChildren, bool isName = true) => DrawBool(_mainProperty.FindPropertyRelative(nameChildren), isName);
        protected bool DrawBool(string nameChildren, string displayName) => DrawBool(_mainProperty.FindPropertyRelative(nameChildren), displayName);
        protected bool DrawBoolRelative(SerializedProperty parent, string nameChildren, bool isName = true) 
        {   return DrawBool(parent.FindPropertyRelative(nameChildren), isName); }
        protected bool DrawBoolRelative(SerializedProperty parent, string nameChildren, string displayName)
        {   return DrawBool(parent.FindPropertyRelative(nameChildren), displayName);}
        #endregion
        //================================================================
        #region DrawInt
        protected int DrawInt(SerializedProperty property, bool isName = true)
        {
            if (isName) return DrawInt(property, property.displayName);

            _position.y += _height;
            return property.intValue = EditorGUI.IntField(_position, property.intValue);
        }
        protected int DrawInt(SerializedProperty property, string displayName)
        {
            _position.y += _height;
            return property.intValue = EditorGUI.IntField(_position, displayName, property.intValue);
        }
        protected int DrawInt(string nameChildren, bool isName = true) => DrawInt(_mainProperty.FindPropertyRelative(nameChildren), isName);
        protected int DrawInt(string nameChildren, string displayName) => DrawInt(_mainProperty.FindPropertyRelative(nameChildren), displayName);
        protected int DrawIntRelative(SerializedProperty parent, string nameChildren, bool isName = true) => DrawInt(parent.FindPropertyRelative(nameChildren), isName);
        protected int DrawIntRelative(SerializedProperty parent, string nameChildren, string displayName)
        {   return DrawInt(parent.FindPropertyRelative(nameChildren), displayName);}
        // == IntSlider ====
        protected int DrawInt(SerializedProperty property, int min, int max, int defaultValue = 0, bool isName = true)
        {
            if (isName) return DrawInt(property, property.displayName, min, max, defaultValue);

            _position.y += _height;

            if (property.intValue < min | property.intValue > max)
                defaultValue = Mathf.Clamp(defaultValue, min, max);
            else
                defaultValue = property.intValue;

            return property.intValue = EditorGUI.IntSlider(_position, Mathf.Clamp(property.intValue, min, max), min, max);
        }
        protected int DrawInt(SerializedProperty property, string displayName, int min, int max, int defaultValue = 0)
        {
            _position.y += _height;

            if (property.intValue < min | property.intValue > max)
                defaultValue = Mathf.Clamp(defaultValue, min, max);
            else
                defaultValue = property.intValue;

            return property.intValue = EditorGUI.IntSlider(_position, displayName, defaultValue, min, max);
        }
        protected int DrawInt(string nameChildren, int min, int max, int defaultValue = 0, bool isName = true) 
        {   return DrawInt(_mainProperty.FindPropertyRelative(nameChildren), min, max, defaultValue, isName); }
        protected int DrawInt(string nameChildren, string displayName, int min, int max, int defaultValue = 0)
        {   return DrawInt(_mainProperty.FindPropertyRelative(nameChildren), displayName, min, max, defaultValue); }
        protected int DrawIntRelative(SerializedProperty parent, string nameChildren, int min, int max, int defaultValue = 0, bool isName = true)
        {   return DrawInt(parent.FindPropertyRelative(nameChildren), min, max, defaultValue, isName); }
        protected int DrawIntRelative(SerializedProperty parent, string nameChildren, string displayName, int min, int max, int defaultValue = 0)
        {   return DrawInt(parent.FindPropertyRelative(nameChildren), displayName, min, max, defaultValue);}
        #endregion
        //================================================================
        #region DrawFloat
        protected float DrawFloat(SerializedProperty property, bool isName = true)
        {
            if (isName) return DrawFloat(property, property.displayName);

            _position.y += _height;
            return property.floatValue = EditorGUI.FloatField(_position, property.floatValue);
        }
        protected float DrawFloat(SerializedProperty property, string displayName)
        {
            _position.y += _height;
            return property.floatValue = EditorGUI.FloatField(_position, displayName, property.floatValue);
        }
        protected float DrawFloat(string nameChildren, bool isName = true) => DrawFloat(_mainProperty.FindPropertyRelative(nameChildren), isName);
        protected float DrawFloat(string nameChildren, string displayName) => DrawFloat(_mainProperty.FindPropertyRelative(nameChildren), displayName);
        protected float DrawFloatRelative(SerializedProperty parent, string nameChildren, bool isName = true)
        { return DrawFloat(parent.FindPropertyRelative(nameChildren), isName); }
        protected float DrawFloatRelative(SerializedProperty parent, string nameChildren, string displayName)
        {   return DrawFloat(parent.FindPropertyRelative(nameChildren), displayName);}
        #endregion
        //================================================================
        #region DrawEnumPopup
        protected T DrawEnumPopup<T>(SerializedProperty property, bool isName = true) where T : Enum
        {
            if (isName) return DrawEnumPopup<T>(property, property.displayName);

            _position.y += _height;
            T value = (T)EditorGUI.EnumPopup(_position, property.enumValueIndex.ToEnum<T>());
            property.enumValueIndex = value.ToInt();
            return value;
        }
        protected T DrawEnumPopup<T>(SerializedProperty property, string displayName) where T : Enum
        {
            _position.y += _height;
            T value = (T)EditorGUI.EnumPopup(_position, displayName, property.enumValueIndex.ToEnum<T>());
            property.enumValueIndex = value.ToInt();
            return value;
        }
        protected T DrawEnumPopup<T>(string nameChildren) where T : Enum => DrawEnumPopup<T>(_mainProperty.FindPropertyRelative(nameChildren));
        protected T DrawEnumPopup<T>(string nameChildren, string displayName) where T : Enum 
        { return DrawEnumPopup<T>(_mainProperty.FindPropertyRelative(nameChildren), displayName); }
        protected T DrawEnumPopupRelative<T>(SerializedProperty parent, string nameChildren) where T : Enum
        { return DrawEnumPopup<T>(parent.FindPropertyRelative(nameChildren)); }
        protected T DrawEnumPopupRelative<T>(SerializedProperty parent, string nameChildren, string displayName) where T : Enum
        {   return DrawEnumPopup<T>(parent.FindPropertyRelative(nameChildren), displayName); }
        #endregion
        //================================================================
        #region DrawIntPopup
        // === Popup ===
        protected int DrawIntPopup(SerializedProperty property, string[] displayedOptions, bool isName = true)
        {
            if (isName) return DrawIntPopup(property, property.displayName, displayedOptions);

            _position.y += _height;
            return property.intValue = EditorGUI.Popup(_position, property.intValue, displayedOptions);
        }
        protected int DrawIntPopup(SerializedProperty property, string displayName, string[] displayedOptions)
        {
            _position.y += _height;
            return property.intValue = EditorGUI.Popup(_position, displayName, property.intValue, displayedOptions);
        }
        protected int DrawIntPopup(string nameChildren, string[] displayedOptions, bool isName = true)
        { return DrawIntPopup(_mainProperty.FindPropertyRelative(nameChildren), displayedOptions, isName); }
        protected int DrawIntPopup(string nameChildren, string displayName, string[] displayedOptions)
        { return DrawIntPopup(_mainProperty.FindPropertyRelative(nameChildren), displayName, displayedOptions); }
        protected int DrawIntPopupRelative(SerializedProperty parent, string nameChildren, string[] displayedOptions, bool isName = true)
        { return DrawIntPopup(parent.FindPropertyRelative(nameChildren), displayedOptions, isName); }
        protected int DrawIntPopupRelative(SerializedProperty parent, string nameChildren, string displayName, string[] displayedOptions)
        { return DrawIntPopup(parent.FindPropertyRelative(nameChildren), displayName, displayedOptions); }
        // === IntPopup ===
        protected int DrawIntPopup(SerializedProperty property, string[] displayedOptions, int[] optionValues, bool isName = true)
        {
            if (isName) return DrawIntPopup(property, property.displayName, displayedOptions, optionValues);

            _position.y += _height;
            return property.intValue = EditorGUI.IntPopup(_position, property.intValue, displayedOptions, optionValues);
        }
        protected int DrawIntPopup(SerializedProperty property, string displayName, string[] displayedOptions, int[] optionValues)
        {
            _position.y += _height;
            return property.intValue = EditorGUI.IntPopup(_position, displayName, property.intValue, displayedOptions, optionValues);
        }
        protected int DrawIntPopup(string nameChildren, string[] displayedOptions, int[] optionValues, bool isName = true)
        { return DrawIntPopup(_mainProperty.FindPropertyRelative(nameChildren), displayedOptions, optionValues, isName); }
        protected int DrawIntPopup(string nameChildren, string displayName, string[] displayedOptions, int[] optionValues)
        { return DrawIntPopup(_mainProperty.FindPropertyRelative(nameChildren), displayName, displayedOptions, optionValues); }
        protected int DrawIntPopupRelative(SerializedProperty parent, string nameChildren, string[] displayedOptions, int[] optionValues, bool isName = true)
        { return DrawIntPopup(parent.FindPropertyRelative(nameChildren), displayedOptions, optionValues, isName); }
        protected int DrawIntPopupRelative(SerializedProperty parent, string nameChildren, string displayName, string[] displayedOptions, int[] optionValues)
        { return DrawIntPopup(parent.FindPropertyRelative(nameChildren), displayName, displayedOptions, optionValues); }
        #endregion
        //================================================================
        #region DrawStringPopup
        protected string DrawStringPopup(SerializedProperty property, string[] displayedOptions, bool isName = true)
        {
            if (isName) return DrawStringPopup(property, property.displayName, displayedOptions);

            _position.y += _height;
            int index = Math.Clamp(Array.IndexOf(displayedOptions, property.stringValue), 0, displayedOptions.Length - 1);
            index = EditorGUI.Popup(_position, index, displayedOptions);
            return property.stringValue = displayedOptions[index];
        }
        protected string DrawStringPopup(SerializedProperty property, string displayName, string[] displayedOptions)
        {
            _position.y += _height;
            int index = Math.Clamp(Array.IndexOf(displayedOptions, property.stringValue), 0, displayedOptions.Length - 1);
            index = EditorGUI.Popup(_position, displayName, index, displayedOptions);
            return property.stringValue = displayedOptions[index];
        }
        protected string DrawStringPopup(string nameChildren, string[] displayedOptions)
        { return DrawStringPopup(_mainProperty.FindPropertyRelative(nameChildren), displayedOptions); }
        protected string DrawStringPopup(string nameChildren, string displayName, string[] displayedOptions)
        { return DrawStringPopup(_mainProperty.FindPropertyRelative(nameChildren), displayName, displayedOptions); }
        protected string DrawStringPopupRelative(SerializedProperty parent, string nameChildren, string[] displayedOptions)
        { return DrawStringPopup(parent.FindPropertyRelative(nameChildren), displayedOptions); }
        protected string DrawStringPopupRelative(SerializedProperty parent, string nameChildren, string displayName, string[] displayedOptions)
        { return DrawStringPopup(parent.FindPropertyRelative(nameChildren), displayName, displayedOptions); }
        #endregion
        //================================================================
        #region DrawId
        protected int DrawId(SerializedProperty property, Type t_field, bool isNone = false, bool isName = true)
        {
            var (names, values) = GetNamesAndValues(t_field, isNone);
            return DrawIntPopup(property, names, values, isName);
        }
        protected int DrawId(SerializedProperty property, string displayName, Type t_field, bool isNone = false)
        {
            var (names, values) = GetNamesAndValues(t_field, isNone);
            return DrawIntPopup(property, displayName, names, values);
        }
        protected int DrawId(string nameChildren, Type t_field, bool isNone = false, bool isName = true)
        {
            var (names, values) = GetNamesAndValues(t_field, isNone);
            return DrawIntPopupRelative(_mainProperty, nameChildren, names, values, isName);
        }
        protected int DrawId(string nameChildren, string displayName, Type t_field, bool isNone = false)
        {
            var (names, values) = GetNamesAndValues(t_field, isNone);
            return DrawIntPopupRelative(_mainProperty, nameChildren, displayName, names, values);
        }
        protected int DrawIdRelative(SerializedProperty parent, string nameChildren, Type t_field, bool isNone = false, bool isName = true)
        {
            var (names, values) = GetNamesAndValues(t_field, isNone);
            return DrawIntPopupRelative(parent, nameChildren, names, values, isName);
        }
        protected int DrawIdRelative(SerializedProperty parent, string nameChildren, string displayName, Type t_field, bool isNone = false)
        {
            var (names, values) = GetNamesAndValues(t_field, isNone);
            return DrawIntPopupRelative(parent, nameChildren, displayName, names, values);
        }
        #endregion
        //================================================================
        #region DrawObject
        protected T DrawObject<T>(SerializedProperty property, bool isName = true) where T : UnityEngine.Object
        {
            if (isName)
                return DrawObject<T>(property, property.displayName);

            _position.y += _height;
            return (T)(property.objectReferenceValue = EditorGUI.ObjectField(_position, property.objectReferenceValue, typeof(T), false));
        }
        protected T DrawObject<T>(SerializedProperty property, string displayName) where T : UnityEngine.Object
        {
            _position.y += _height;
            return (T)(property.objectReferenceValue = EditorGUI.ObjectField(_position, displayName, property.objectReferenceValue, typeof(T), false));
        }
        protected T DrawObject<T>(string nameChildren, bool isName = true) where T : UnityEngine.Object
        { return DrawObject<T>(_mainProperty.FindPropertyRelative(nameChildren), isName); }
        protected T DrawObject<T>(string nameChildren, string displayName) where T : UnityEngine.Object
        { return DrawObject<T>(_mainProperty.FindPropertyRelative(nameChildren), displayName); }
        protected T DrawObjectRelative<T>(SerializedProperty parent, string nameChildren, bool isName = true) where T : UnityEngine.Object
        { return DrawObject<T>(parent.FindPropertyRelative(nameChildren), isName); }
        protected T DrawObjectRelative<T>(SerializedProperty parent, string nameChildren, string displayName) where T : UnityEngine.Object
        { return DrawObject<T>(parent.FindPropertyRelative(nameChildren), displayName); }
        #endregion
        //================================================================
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

        protected SerializedProperty GetProperty(string nameChildren) => _mainProperty.FindPropertyRelative(nameChildren);
        protected SerializedProperty GetProperty(SerializedProperty parent, string nameChildren) => parent.FindPropertyRelative(nameChildren);

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

        protected bool TrySetArraySize(SerializedProperty property, int size)
        {
            if(size < 0 || property == null || !property.isArray)
                return false;

            while (property.arraySize > size)
                property.DeleteArrayElementAtIndex(property.arraySize - 1);
            while (property.arraySize < size)
                property.InsertArrayElementAtIndex(property.arraySize);

            return true;
        }
    }
}
