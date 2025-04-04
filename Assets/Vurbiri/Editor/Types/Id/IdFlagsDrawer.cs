//Assets\Vurbiri\Editor\Types\Id\IdFlagsDrawer.cs
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
        #region Consts
        private const int MAX_COUNT = 32;
        private const string P_NAME = "_id";
		#endregion
		
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

            string[] names = null;
            if(!GetNames(ref names))
            {
                HelpBox(position, $"Error getting values", UnityEditor.MessageType.Error);
                return;
            }
            if(names.Length > MAX_COUNT)
            {
                HelpBox(position, $"Count of flags is greater than {MAX_COUNT}", UnityEditor.MessageType.Error);
                return;
            }

            SerializedProperty valueProperty = mainProperty.FindPropertyRelative(P_NAME);
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

        private bool GetNames(ref string[] names)
        {
            Type typeField = fieldInfo.FieldType;
            if (typeField.IsArray) typeField = typeField.GetElementType();

            Type[] arg = typeField.GetGenericArguments();
            if (arg == null || arg.Length != 1) return false;
            typeField = typeField.GetGenericArguments()[0].BaseType;

            PropertyInfo namesProperty = null;
            while (typeField != null & namesProperty == null)
            {
                namesProperty = typeField.GetProperty("Names");
                typeField = typeField.BaseType;
            }

            if (namesProperty == null) 
                return false;

            names = (string[])namesProperty.GetValue(null);

            return names != null;
        }
    }
}