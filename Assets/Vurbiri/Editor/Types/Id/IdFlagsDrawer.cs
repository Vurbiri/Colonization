using System;
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

        private Type _type;
        private int _count;
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
            
            if (_count > MAX_COUNT)
            {
                HelpBox(position, $"Count of flags is greater than {MAX_COUNT}", UnityEditor.MessageType.Error); return;
            }

            SerializedProperty valueProperty = mainProperty.FindPropertyRelative(P_NAME);
            BeginProperty(position, label, mainProperty);
            {
                valueProperty.intValue = MaskField(position, label, valueProperty.intValue, _names) & ~(-1 << _count);
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

        private bool TryGetNames(Type idType)
        {
            if(!(IdTypesCacheEditor.Contain(idType) && IdTypesCacheEditor.GetMin(idType) >= 0))
                return false;

            bool isInit = _type == idType & _names != null;

            if (!isInit)
            {
                _type = idType;
                _count = IdTypesCacheEditor.GetCount(idType);
                _names = IdTypesCacheEditor.GetNames(idType);
                isInit = true;
            }

            return isInit;
        }
    }
}
