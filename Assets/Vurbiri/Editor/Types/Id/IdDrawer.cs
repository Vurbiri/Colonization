using System;
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

        private Type _type;
        private string[] _names;
        private int[] _values;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!TryGetType(out Type idType))
            {
                HelpBox(position, "Failed to determine type", UnityEditor.MessageType.Error); return;
            }
            if (!TryGetNamesAndValues(idType))
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

        private bool TryGetType(out Type idType)
        {
            idType = fieldInfo.FieldType;
            if (idType.IsArray) idType = idType.GetElementType();

            Type[] arg = idType.GetGenericArguments();
            if (arg == null || arg.Length != 1) return false;
            idType = idType.GetGenericArguments()[0];

            return idType != null;
        }

        private bool TryGetNamesAndValues(Type idType)
        {
            bool isInit = _type == idType & _names != null & _values != null;

            if (!isInit && IdTypesCache.Contain(idType))
            {
                _type = idType;
                _names = IdTypesCache.GetDisplayNames(idType);
                _values = IdTypesCache.GetValues(idType);
                isInit = true;
            }

            return isInit;
        }
    }
}
