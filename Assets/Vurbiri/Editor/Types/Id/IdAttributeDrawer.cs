using System;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(IdAttribute))]
    public class IdAttributeDrawer : PropertyDrawer
    {
        private Type _type;
        private string[] _names;
        private int[] _values;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                HelpBox(position, "Field type must be Int32", UnityEditor.MessageType.Error);
                return;
            }
            if (!TryGetNamesAndValues())
            {
                HelpBox(position, $"Error values", UnityEditor.MessageType.Error);
                return;
            }

            label = BeginProperty(position, label, property);
            {
                property.intValue = IntPopup(position, label.text, property.intValue, _names, _values, EditorStyles.popup);
            }
            EndProperty();
        }

        private bool TryGetNamesAndValues()
        {
            Type idType = ((IdAttribute)attribute).type;

            bool isInit = _type == idType & _names != null & _values != null;

            if (!isInit)
            {
                _type = idType;
                _names = IdTypesCacheEditor.GetDisplayNames(idType);
                _values = IdTypesCacheEditor.GetValues(idType);
                isInit = true;
            }

            return isInit;
        }
    }
}
