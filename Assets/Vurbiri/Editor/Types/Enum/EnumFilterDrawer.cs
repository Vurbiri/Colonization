using System;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace Vurbiri.Editor
{
    [CustomPropertyDrawer(typeof(EnumFilter<>))]
	public class EnumFilterDrawer : PropertyDrawer
	{
        private readonly int MAX_COUNT = 32;
        private readonly string P_VALUE = "_value";

        private Type _type;
        private string[] _names;

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            if (!TryGetTypeEnum(position, out Type enumType)) return;

            if (enumType != _type & _names == null)
            {
                _type = enumType;
                _names = enumType.GetEnumNames();
            }
            int count = _names.Length;
            if (count > MAX_COUNT)
            {
                HelpBox(position, $"Count of values is greater than {MAX_COUNT}", UnityEditor.MessageType.Error);
                return;
            }

            SerializedProperty valueProperty = mainProperty.FindPropertyRelative(P_VALUE);

            BeginProperty(position, label, mainProperty);
            {
                valueProperty.intValue = MaskField(position, label, valueProperty.intValue, _names) & ~(-1 << count);
            }
            EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
        {
            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 1f;
        }

        private bool TryGetTypeEnum(Rect position, out Type enumType)
        {
            enumType = fieldInfo.FieldType;
            if (enumType.IsArray) enumType = enumType.GetElementType();

            Type[] arguments = enumType.GetGenericArguments();
            if (arguments != null && arguments.Length == 1)
            {
                enumType = enumType.GetGenericArguments()[0];
                if (enumType != null && enumType.IsEnum)
                    return true;
            }

            HelpBox(position, "Failed to determine type", UnityEditor.MessageType.Error);
            return false;
        }
    }
}
