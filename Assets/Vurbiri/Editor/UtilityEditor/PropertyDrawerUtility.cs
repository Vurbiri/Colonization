using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
    public abstract class PropertyDrawerUtility : PropertyDrawer
    {
 
        protected Rect _position;
        protected SerializedProperty _mainProperty;
        protected GUIContent _label;
        protected readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        protected readonly float _ySpace = EditorGUIUtility.standardVerticalSpacing;

        sealed public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            
            _position = position;
            _mainProperty = mainProperty;
            _label = label;

            OnGUI();
        }

        protected abstract void OnGUI();


        //================================================================
        protected void Space(float ratio = 1f) => _position.y += _ySpace * ratio;

        protected void BeginProperty() => _label = EditorGUI.BeginProperty(_position, _label, _mainProperty);

        protected bool Foldout() => Foldout(_label);
        protected bool Foldout(GUIContent label) => _mainProperty.isExpanded = EditorGUI.Foldout(_position, _mainProperty.isExpanded, label);

        //================================================================
        #region Property
        protected bool DrawProperty(bool includeChildren = false)
        {
            _position.y += _height;
            return PropertyField(_position, _mainProperty, includeChildren);
        }
        protected bool DrawProperty(SerializedProperty property, bool includeChildren = false)
        {
            _position.y += _height;
            return PropertyField(_position, property, includeChildren);
        }
        protected bool DrawProperty(string nameChildren, bool includeChildren = false)
        {
            _position.y += _height;
            return PropertyField(_position, _mainProperty.FindPropertyRelative(nameChildren), includeChildren);
        }
        #endregion
        //================================================================
        #region Bool
        #region DrawBool
        protected bool DrawBool(SerializedProperty property, bool isName = true)
        {
            if (isName) return DrawBool(property, property.displayName);

            _position.y += _height;
            return property.boolValue = Toggle(_position, property.boolValue);
        }
        protected bool DrawBool(SerializedProperty property, string displayName)
        {
            _position.y += _height;
            return property.boolValue = Toggle(_position, displayName, property.boolValue);
        }
        protected bool DrawBool(string nameChildren, bool isName = true) => DrawBool(_mainProperty.FindPropertyRelative(nameChildren), isName);
        protected bool DrawBool(string nameChildren, string displayName) => DrawBool(_mainProperty.FindPropertyRelative(nameChildren), displayName);
        protected bool DrawBoolRelative(SerializedProperty parent, string nameChildren, bool isName = true) 
        {   return DrawBool(parent.FindPropertyRelative(nameChildren), isName); }
        protected bool DrawBoolRelative(SerializedProperty parent, string nameChildren, string displayName)
        {   return DrawBool(parent.FindPropertyRelative(nameChildren), displayName);}
        #endregion
        //----------------------------------------------------------------
        #region GetBool, SetBool
        protected bool GetBool(string nameChildren) => _mainProperty.FindPropertyRelative(nameChildren).boolValue;

        protected void SetBool(string nameChildren, bool value) => _mainProperty.FindPropertyRelative(nameChildren).boolValue = value;
        #endregion
        //----------------------------------------------------------------
        #region SetLabelBool
        protected void SetLabelBool(string nameChildren, bool value) => SetLabelBool(_mainProperty, nameChildren, value);
        protected void SetLabelBool(SerializedProperty parent, string nameChildren, bool value)
        {
            SerializedProperty property = parent.FindPropertyRelative(nameChildren);

            property.boolValue = value;

            _position.y += _height;
            EditorGUI.LabelField(_position, property.displayName, value.ToString());
        }
        #endregion
        #endregion
        //================================================================
        #region Int
        #region DrawInt
        protected int DrawInt(SerializedProperty property, bool isName = true)
        {
            if (isName) return DrawInt(property, property.displayName);

            _position.y += _height;
            return property.intValue = IntField(_position, property.intValue);
        }
        protected int DrawInt(SerializedProperty property, string displayName)
        {
            _position.y += _height;
            return property.intValue = IntField(_position, displayName, property.intValue);
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

            if (property.intValue < min | property.intValue > max)
                defaultValue = Mathf.Clamp(defaultValue, min, max);
            else
                defaultValue = property.intValue;

            _position.y += _height;
            return property.intValue = IntSlider(_position, defaultValue, min, max);
        }
        protected int DrawInt(SerializedProperty property, string displayName, int min, int max, int defaultValue = 0)
        {
            if (property.intValue < min | property.intValue > max)
                defaultValue = Mathf.Clamp(defaultValue, min, max);
            else
                defaultValue = property.intValue;

            _position.y += _height;
            return property.intValue = IntSlider(_position, displayName, defaultValue, min, max);
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
        //----------------------------------------------------------------
        #region GetInt, SetInt
        protected int GetInt(string nameChildren) => _mainProperty.FindPropertyRelative(nameChildren).intValue;

        protected void SetInt(string nameChildren, int value) => _mainProperty.FindPropertyRelative(nameChildren).intValue = value;
        #endregion
        //----------------------------------------------------------------
        #region SetLabelInt
        protected void SetLabelInt(string nameChildren, int value) => SetLabelInt(_mainProperty, nameChildren, value);
        protected void SetLabelInt(SerializedProperty parent, string nameChildren, int value)
        {
            SerializedProperty property = parent.FindPropertyRelative(nameChildren);

            property.intValue = value;

            _position.y += _height;
            LabelField(_position, property.displayName, value.ToString());
        }
        #endregion
        #endregion
        //================================================================
        #region Float
        #region DrawFloat
        protected float DrawFloat(SerializedProperty property, bool isName = true)
        {
            if (isName) return DrawFloat(property, property.displayName);

            _position.y += _height;
            return property.floatValue = FloatField(_position, property.floatValue);
        }
        protected float DrawFloat(SerializedProperty property, string displayName)
        {
            _position.y += _height;
            return property.floatValue = FloatField(_position, displayName, property.floatValue);
        }
        protected float DrawFloat(string nameChildren, bool isName = true) => DrawFloat(_mainProperty.FindPropertyRelative(nameChildren), isName);
        protected float DrawFloat(string nameChildren, string displayName) => DrawFloat(_mainProperty.FindPropertyRelative(nameChildren), displayName);
        protected float DrawFloatRelative(SerializedProperty parent, string nameChildren, bool isName = true)
        { return DrawFloat(parent.FindPropertyRelative(nameChildren), isName); }
        protected float DrawFloatRelative(SerializedProperty parent, string nameChildren, string displayName)
        {   return DrawFloat(parent.FindPropertyRelative(nameChildren), displayName);}
        // == Slider ====
        protected float DrawFloat(SerializedProperty property, float min, float max, float defaultValue = 0f, bool isName = true)
        {
            if (isName) return DrawFloat(property, property.displayName, min, max, defaultValue);

            if (property.intValue < min | property.intValue > max)
                defaultValue = Mathf.Clamp(defaultValue, min, max);
            else
                defaultValue = property.floatValue;

            _position.y += _height;
            return property.floatValue = Slider(_position, defaultValue, min, max);
        }
        protected float DrawFloat(SerializedProperty property, string displayName, float min, float max, float defaultValue = 0f)
        {
            if (property.floatValue < min | property.floatValue > max)
                defaultValue = Mathf.Clamp(defaultValue, min, max);
            else
                defaultValue = property.floatValue;

            _position.y += _height;
            return property.floatValue = Slider(_position, displayName, defaultValue, min, max);
        }
        protected float DrawFloat(string nameChildren, float min, float max, float defaultValue = 0f, bool isName = true)
        { return DrawFloat(_mainProperty.FindPropertyRelative(nameChildren), min, max, defaultValue, isName); }
        protected float DrawFloat(string nameChildren, string displayName, float min, float max, float defaultValue = 0f)
        { return DrawFloat(_mainProperty.FindPropertyRelative(nameChildren), displayName, min, max, defaultValue); }
        protected float DrawFloatRelative(SerializedProperty parent, string nameChildren, float min, float max, float defaultValue = 0f, bool isName = true)
        { return DrawFloat(parent.FindPropertyRelative(nameChildren), min, max, defaultValue, isName); }
        protected float DrawFloatRelative(SerializedProperty parent, string nameChildren, string displayName, float min, float max, float defaultValue = 0f)
        { return DrawFloat(parent.FindPropertyRelative(nameChildren), displayName, min, max, defaultValue); }
        #endregion
        //----------------------------------------------------------------
        #region GetFloat, SetFloat
        protected float GetFloat(string nameChildren) => _mainProperty.FindPropertyRelative(nameChildren).floatValue;

        protected void SetFloat(string nameChildren, float value) => _mainProperty.FindPropertyRelative(nameChildren).floatValue = value;
        #endregion
        //----------------------------------------------------------------
        #region SetLabelFloat
        protected void SetLabelFloat(string nameChildren, float value) => SetLabelFloat(_mainProperty, nameChildren, value);
        protected void SetLabelFloat(SerializedProperty parent, string nameChildren, float value) => SetLabelFloat(parent.FindPropertyRelative(nameChildren), value);
        protected void SetLabelFloat(SerializedProperty property, float value)
        {
            property.floatValue = value;

            _position.y += _height;
            LabelField(_position, property.displayName, value.ToString());
        }
        #endregion
        #endregion
        //================================================================
        #region Enum
        #region DrawEnum
        protected T DrawEnum<T>(SerializedProperty property, bool isName = true) where T : Enum
        {
            if (isName) return DrawEnum<T>(property, property.displayName);

            _position.y += _height;
            T value = (T)EnumPopup(_position, property.enumValueIndex.ToEnum<T>());
            property.enumValueIndex = value.ToInt();
            return value;
        }
        protected T DrawEnum<T>(SerializedProperty property, string displayName) where T : Enum
        {
            _position.y += _height;
            T value = (T)EnumPopup(_position, displayName, property.enumValueIndex.ToEnum<T>());
            property.enumValueIndex = value.ToInt();
            return value;
        }
        protected T DrawEnum<T>(string nameChildren) where T : Enum => DrawEnum<T>(_mainProperty.FindPropertyRelative(nameChildren));
        protected T DrawEnum<T>(string nameChildren, string displayName) where T : Enum 
        { return DrawEnum<T>(_mainProperty.FindPropertyRelative(nameChildren), displayName); }
        protected T DrawEnumRelative<T>(SerializedProperty parent, string nameChildren) where T : Enum
        { return DrawEnum<T>(parent.FindPropertyRelative(nameChildren)); }
        protected T DrawEnumRelative<T>(SerializedProperty parent, string nameChildren, string displayName) where T : Enum
        {   return DrawEnum<T>(parent.FindPropertyRelative(nameChildren), displayName); }
        #endregion
        //----------------------------------------------------------------
        #region GetEnum, SetEnum
        protected T GetEnum<T>(string nameChildren) where T : Enum => _mainProperty.FindPropertyRelative(nameChildren).enumValueIndex.ToEnum<T>();

        protected void SetEnum<T>(string nameChildren, T value) where T : Enum => _mainProperty.FindPropertyRelative(nameChildren).enumValueIndex = value.ToInt();
        #endregion
        //----------------------------------------------------------------
        #region SetLabelBool
        protected void SetLabelEnum<T>(string nameChildren, T value) where T : Enum
        { SetLabelEnum(_mainProperty.FindPropertyRelative(nameChildren), nameChildren, value); }
        protected void SetLabelEnum<T>(SerializedProperty parent, string nameChildren, T value) where T : Enum
        { SetLabelEnum(parent.FindPropertyRelative(nameChildren), nameChildren, value); }
        protected void SetLabelEnum<T>(SerializedProperty property, T value) where T : Enum
        {
            int index = value.ToInt();
            property.enumValueIndex = index;

            _position.y += _height;
            LabelField(_position, property.displayName, property.enumDisplayNames[index]);
        }
        #endregion
        #endregion
        //================================================================
        #region DrawIntPopup
        // === Popup ===
        protected int DrawIntPopup(SerializedProperty property, string[] displayedOptions, bool isName = true)
        {
            if (isName) return DrawIntPopup(property, property.displayName, displayedOptions);

            _position.y += _height;
            return property.intValue = Popup(_position, property.intValue, displayedOptions);
        }
        protected int DrawIntPopup(SerializedProperty property, string displayName, string[] displayedOptions)
        {
            _position.y += _height;
            return property.intValue = Popup(_position, displayName, property.intValue, displayedOptions);
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
            return property.intValue = IntPopup(_position, displayName, property.intValue, displayedOptions, optionValues);
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
            index = Popup(_position, index, displayedOptions);
            return property.stringValue = displayedOptions[index];
        }
        protected string DrawStringPopup(SerializedProperty property, string displayName, string[] displayedOptions)
        {
            _position.y += _height;
            int index = Math.Clamp(Array.IndexOf(displayedOptions, property.stringValue), 0, displayedOptions.Length - 1);
            index = Popup(_position, displayName, index, displayedOptions);
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
        protected int DrawId<T>(SerializedProperty property, bool isNone = false, bool isName = true) where T : IdType<T>
        {
            var (names, values) = GetNamesAndValues<T>(isNone);
            return DrawIntPopup(property, names, values, isName);
        }
        protected int DrawId<T>(SerializedProperty property, string displayName, bool isNone = false) where T : IdType<T>
        {
            var (names, values) = GetNamesAndValues<T>(isNone);
            return DrawIntPopup(property, displayName, names, values);
        }
        protected int DrawId<T>(string nameChildren, bool isNone = false, bool isName = true) where T : IdType<T>
        {
            var (names, values) = GetNamesAndValues<T>(isNone);
            return DrawIntPopupRelative(_mainProperty, nameChildren, names, values, isName);
        }
        protected int DrawId<T>(string nameChildren, string displayName, bool isNone = false) where T : IdType<T>
        {
            var (names, values) = GetNamesAndValues<T>(isNone);
            return DrawIntPopupRelative(_mainProperty, nameChildren, displayName, names, values);
        }
        protected int DrawIdRelative<T>(SerializedProperty parent, string nameChildren, bool isNone = false, bool isName = true) where T : IdType<T>
        {
            var (names, values) = GetNamesAndValues<T>(isNone);
            return DrawIntPopupRelative(parent, nameChildren, names, values, isName);
        }
        protected int DrawIdRelative<T>(SerializedProperty parent, string nameChildren, string displayName, bool isNone = false) where T : IdType<T>
        {
            var (names, values) = GetNamesAndValues<T>(isNone);
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
            return (T)(property.objectReferenceValue = ObjectField(_position, property.objectReferenceValue, typeof(T), false));
        }
        protected T DrawObject<T>(SerializedProperty property, string displayName) where T : UnityEngine.Object
        {
            _position.y += _height;
            return (T)(property.objectReferenceValue = ObjectField(_position, displayName, property.objectReferenceValue, typeof(T), false));
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
        #region DrawLabel
        protected void DrawLabel(string displayName)
        {
            _position.y += _height;
            LabelField(_position, displayName);
        }
        protected void DrawLabel(string displayName, string value)
        {
            _position.y += _height;
            LabelField(_position, displayName, value);
        }
        #endregion
        //================================================================
        #region DrawLine
        protected void DrawLine() => DrawLine(Color.gray, 0f);
        protected void DrawLine(float leftOffset) => DrawLine(Color.gray, leftOffset);
        protected void DrawLine(Color color, float leftOffset = 0f)
        {
            Rect size = _position;
            size.y += _height + _ySpace * 2f;
            size.x += leftOffset;
            size.width -= leftOffset;
            size.height = _ySpace;
            DrawRect(size, color);
            _position.y += _ySpace * 5f;
        }
        #endregion
        //================================================================
        #region GetProperty
        protected SerializedProperty GetProperty(string nameChildren) => _mainProperty.FindPropertyRelative(nameChildren);
        protected SerializedProperty GetProperty(SerializedProperty parent, string nameChildren) => parent.FindPropertyRelative(nameChildren);
        #endregion
        //================================================================
        #region Utilities

        protected (string[] names, int[] values) GetNamesAndValues<T>(bool isNone) where T : IdType<T>
        {
            if (!isNone)
                return (IdType<T>.Names, IdType<T>.Values);

            List<string> names = new(IdType<T>.Names);
            List<int> values = new(IdType<T>.Values);

            names.Insert(0, "None");
            values.Insert(0, -1);

            return (names.ToArray(), values.ToArray());
        }

        protected int IdFromLabel() => IdFromLabel(_label);
        protected int IdFromLabel(GUIContent label)
        {
            string[] strings = label.text.Split(' ');
            int id = -1;
            if (strings.Length == 2)
                id = int.Parse(strings[1]);

            return id;
        }
        #endregion
    }

    //================================================================================================================================
    //===================================== Extensions SerializedProperty ============================================================
    //================================================================================================================================
    public static class ExtensionsSerializedProperty
    {
        //================================================================
        #region Bool
        public static bool GetBool(this SerializedProperty parent, string nameChildren) => parent.FindPropertyRelative(nameChildren).boolValue;
        public static void SetBool(this SerializedProperty parent, string nameChildren, bool value) => parent.FindPropertyRelative(nameChildren).boolValue = value;
        #endregion
        //================================================================
        #region Int
        public static int GetInt(this SerializedProperty parent, string nameChildren) => parent.FindPropertyRelative(nameChildren).intValue;
        public static void SetInt(this SerializedProperty parent, string nameChildren, int value) => parent.FindPropertyRelative(nameChildren).intValue = value;
        #endregion
        //================================================================
        #region Float
        public static float GetFloat(this SerializedProperty parent, string nameChildren) => parent.FindPropertyRelative(nameChildren).floatValue;
        public static void SetFloat(this SerializedProperty parent, string nameChildren, float value) => parent.FindPropertyRelative(nameChildren).floatValue = value;
        #endregion
        //================================================================
        #region Enum
        public static T GetEnum<T>(this SerializedProperty property) where T : Enum => property.enumValueIndex.ToEnum<T>();
        public static T GetEnum<T>(this SerializedProperty parent, string nameChildren) where T : Enum
        {   return parent.FindPropertyRelative(nameChildren).enumValueIndex.ToEnum<T>(); }

        public static void SetEnum(this SerializedProperty property, Enum value) => property.enumValueIndex = value.ToInt();
        public static void SetEnum(this SerializedProperty parent, string nameChildren, Enum value)
        { parent.FindPropertyRelative(nameChildren).enumValueIndex = value.ToInt();}
        #endregion
        //================================================================
    }
}
