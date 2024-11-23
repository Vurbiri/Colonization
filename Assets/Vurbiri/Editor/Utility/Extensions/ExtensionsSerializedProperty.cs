//Assets\Vurbiri\Editor\Utility\Extensions\ExtensionsSerializedProperty.cs
using System;
using UnityEditor;
using Vurbiri;

namespace VurbiriEditor
{
    public static class ExtensionsSerializedProperty
	{
		public static T GetEnumValue<T>(this SerializedProperty property) where T : Enum => property.enumValueIndex.ToEnum<T>();
        public static void SetEnumValue(this SerializedProperty property, Enum value) => property.enumValueIndex = value.ToInt();
    }
}
