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
            if (!IdCacheEd.Contain(idType))
            {
                HelpBox(position, $"Error type", UnityEditor.MessageType.Error);
                return;
            }

            label = BeginProperty(position, label, property);
            {
                property.intValue = IntPopup(position, label.text, property.intValue, IdCacheEd.GetNamesNone(idType), IdCacheEd.GetValuesNone(idType), EditorStyles.popup);
            }
            EndProperty();
        }
    }
}
