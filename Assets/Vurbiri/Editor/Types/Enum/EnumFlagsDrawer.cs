using System;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace Vurbiri.Editor
{
    [CustomPropertyDrawer(typeof(EnumFlags<>))]
    public class EnumFlagsDrawer : PropertyDrawer
	{
        #region Consts
        private const int MAX_COUNT = 32;
        private const string P_VALUE = "_value";
        #endregion

        private Type _type;
        private string[] _names;

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

            if (!TryGetTypeEnum(out Type enumType))
            {
                HelpBox(position, "Type definition error", UnityEditor.MessageType.Error);
                return;
            }

            if (enumType != _type || _names == null)
            {
                _type = enumType;
                _names = enumType.GetEnumNames();
                if (_names.Length > MAX_COUNT)
                {
                    HelpBox(position, $"Count of flags is greater than {MAX_COUNT}", UnityEditor.MessageType.Error);
                    return;
                }
            }

            SerializedProperty valueProperty = mainProperty.FindPropertyRelative(P_VALUE);

            BeginProperty(position, label, mainProperty);
            {
                valueProperty.intValue = MaskField(position, label, valueProperty.intValue, _names) & ~(-1 << _names.Length);
            }
            EndProperty();
        }
		
		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		}

		private bool TryGetTypeEnum(out Type enumType)
		{
            var fieldType = fieldInfo.FieldType;
			if (fieldType.IsArray) fieldType = fieldType.GetElementType();

			var arguments = fieldType.GetGenericArguments();
			if (arguments != null && arguments.Length == 1)
			{
				enumType = arguments[0];
                if (VerificationType(enumType))
                    return true;
            }

            enumType = null;
            return false;
        }

		private static bool VerificationType(Type enumType)
		{
            if(enumType == null || !enumType.IsEnum)
                return false;
            
            int value, oldValue = -1;
            foreach (var em in enumType.GetEnumValues())
			{
                value = Convert.ToInt32(em);
                if ((value - oldValue) != 1)
                    return false;

                oldValue = value;
            }
			return true;
		}
    }
}
