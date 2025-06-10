using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(Id<>), true)]
    public class IdDrawer : PropertyDrawer
    {
        private readonly string NAME_VALUE = "_id";
        private readonly string TP_DNAMES = "DisplayNames", TP_VALUES = "Values";

        private Type _type;
        private string[] _names;
        private int[] _values;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!TryGetType(out Type typeId))
            {
                HelpBox(position, "Failed to determine type", UnityEditor.MessageType.Error); return;
            }
            if (!TryGetNamesAndValues(typeId))
            {
                HelpBox(position, $"Error values", UnityEditor.MessageType.Error); return;
            }

            SerializedProperty valueProperty = property.FindPropertyRelative(NAME_VALUE);
            label = BeginProperty(position, label, property);
            {
                valueProperty.intValue = IntPopup(position, label.text, valueProperty.intValue, _names, _values, EditorStyles.popup);
            }
            EndProperty();
        }

        private bool TryGetType(out Type typeId)
        {
            typeId = fieldInfo.FieldType;
            if (typeId.IsArray) typeId = typeId.GetElementType();

            Type[] arg = typeId.GetGenericArguments();
            if (arg == null || arg.Length != 1) return false;
            typeId = typeId.GetGenericArguments()[0];

            return typeId != null;
        }

        private bool TryGetNamesAndValues(Type typeId)
        {
            if (typeId == _type & _names != null & _values != null) return true;

            _type = typeId;

            typeId = typeId.BaseType;
            PropertyInfo displayNamesProperty = null, valuesProperty = null;
            while (typeId != null & (displayNamesProperty == null | valuesProperty == null))
            {
                displayNamesProperty = typeId.GetProperty(TP_DNAMES);
                valuesProperty = typeId.GetProperty(TP_VALUES);
                typeId = typeId.BaseType;
            }

            if (displayNamesProperty == null | valuesProperty == null) return false;

            _names = (string[])displayNamesProperty.GetValue(null);
            _values = (int[])valuesProperty.GetValue(null);
            return _names != null & _values != null;
        }
    }
}
