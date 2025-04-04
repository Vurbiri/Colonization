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
		private const string P_NAME = "_id";
		#endregion
		
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty valueProperty = mainProperty.FindPropertyRelative(P_NAME);
            string[] names = GetNames();

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

        private string[] GetNames()
        {
            Type typeField = fieldInfo.FieldType;
            PropertyInfo names;

            if (typeField.IsArray)
                typeField = typeField.GetElementType();

            typeField = typeField.GetGenericArguments()[0];

            do
            {
                typeField = typeField.BaseType;
                names = typeField.GetProperty("Names");
            }
            while (typeField != null & names == null);

            return (string[])names.GetValue(null);
        }
    }
}