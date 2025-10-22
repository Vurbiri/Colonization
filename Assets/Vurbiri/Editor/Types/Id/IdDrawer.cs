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

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!TryGetType(out Type idType))
            {
                HelpBox(position, "Failed to determine type", UnityEditor.MessageType.Error); 
                return;
            }
            if (!IdCacheEd.Contain(idType))
            {
                HelpBox(position, $"Error type", UnityEditor.MessageType.Error); 
                return;
            }

            SerializedProperty valueProperty = property.FindPropertyRelative(NAME_VALUE);
            label = BeginProperty(position, label, property);
            {
                valueProperty.intValue = IntPopup(position, label.text, valueProperty.intValue, IdCacheEd.GetNamesNone(idType), IdCacheEd.GetValuesNone(idType));
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
    }
}
