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
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                HelpBox(position, "Field type must be Int32", UnityEditor.MessageType.Error);
                return;
            }

            Type idType = ((IdAttribute)attribute).type;
            if (!IdTypeCache.Contain(idType))
            {
                HelpBox(position, $"Error type", UnityEditor.MessageType.Error);
                return;
            }

            label = BeginProperty(position, label, property);
            {
                property.intValue = IntPopup(position, label.text, property.intValue, IdTypeCache.GetNamesNone(idType), IdTypeCache.GetValuesNone(idType), EditorStyles.popup);
            }
            EndProperty();
        }
    }
}
