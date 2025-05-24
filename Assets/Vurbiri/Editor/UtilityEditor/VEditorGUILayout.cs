using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VurbiriEditor
{
    public static class VEditorGUILayout
	{
        public static T ObjectField<T>(string label, T obj, bool allowSceneObjects = true) where T : Object
        {
            return EditorGUILayout.ObjectField(label, obj, typeof(T), allowSceneObjects) as T;
        }
        public static T ObjectField<T>(GUIContent label, T obj, bool allowSceneObjects = true) where T : Object
        {
            return EditorGUILayout.ObjectField(label, obj, typeof(T), allowSceneObjects) as T;
        }

        public static T EnumPopup<T>(string label, T selected) where T : Enum => (T)EditorGUILayout.EnumPopup(label, selected);
    }
}
