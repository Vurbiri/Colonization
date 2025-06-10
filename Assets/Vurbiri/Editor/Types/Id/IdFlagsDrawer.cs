using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(IdFlags<>))]
	public class IdFlagsDrawer : PropertyDrawer
	{
        private readonly int MAX_COUNT = 32;
        private readonly string P_NAME = "_id";
        private readonly string TF_MIN = "Min", TP_NAMES = "Names";

        private Type _type;
        private string[] _names;

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

            if(!TryGetType(out Type typeId))
            {
                HelpBox(position, "Failed to determine type", UnityEditor.MessageType.Error); return;
            }
            if (!TryGetNames(typeId))
            {
                HelpBox(position, $"Error values", UnityEditor.MessageType.Error); return;
            }
            int count = _names.Length;
            if (count > MAX_COUNT)
            {
                HelpBox(position, $"Count of flags is greater than {MAX_COUNT}", UnityEditor.MessageType.Error); return;
            }

            SerializedProperty valueProperty = mainProperty.FindPropertyRelative(P_NAME);
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

        private bool TryGetType(out Type typeId)
        {
            typeId = fieldInfo.FieldType;
            if (typeId.IsArray) typeId = typeId.GetElementType();

            Type[] arg = typeId.GetGenericArguments();
            if (arg == null || arg.Length != 1) return false;
            typeId = typeId.GetGenericArguments()[0];

            return typeId != null;
        }

        private bool TryGetNames(Type typeId)
        {
            if (typeId == _type & _names != null) return true;

            _type = typeId;

            typeId = typeId.BaseType;
            PropertyInfo namesProperty = null; FieldInfo minField = null;
            while (typeId != null & (namesProperty == null | minField == null))
            {
                minField = typeId.GetField(TF_MIN);
                namesProperty = typeId.GetProperty(TP_NAMES);
                typeId = typeId.BaseType;
            }

            if (namesProperty == null | minField == null || (int)minField.GetValue(null) < 0) return false;
            _names = (string[])namesProperty.GetValue(null);
            return _names != null;
        }
    }
}
