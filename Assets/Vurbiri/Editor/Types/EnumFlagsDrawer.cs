//Assets\Vurbiri\Editor\Types\EnumFlagsDrawer.cs
using System;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(EnumFlags<>))]
    public class EnumFlagsDrawer : PropertyDrawer
	{
		#region Consts
		private const string P_VALUE = "_value", P_COUNT = "_count";
		#endregion

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

            if (!TryGetTypeEnum(position, out Type enumType)) return;

            string[] names = enumType.GetEnumNames();
            mainProperty.FindPropertyRelative(P_COUNT).intValue = names.Length;

            SerializedProperty valueProperty = mainProperty.FindPropertyRelative(P_VALUE);

            BeginProperty(position, label, mainProperty);
            {
                valueProperty.intValue = MaskField(position, label, valueProperty.intValue, names);
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
				{
					if(VerificationValues(enumType)) 
						return true;

                    enumType = null;
                    HelpBox(position, "Values cannot be negative or out of order", UnityEditor.MessageType.Error);
                    return false;
                }
            }

            HelpBox(position, "Failed to determine type", UnityEditor.MessageType.Error);
            return false;
        }
		private bool VerificationValues(Type enumType)
		{
			int intValue, oldValue = -1;
			foreach (var value in enumType.GetEnumValues())
			{
                intValue = (int)value; 
                if ((intValue - oldValue) != 1) 
					return false;
                
				oldValue = intValue;
            }
			return true;
		}
    }
}