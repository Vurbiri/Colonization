using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GetComponentsInChildrenAttribute))]
public class GetComponentsInChildrenAttributeDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Type typeProperty = fieldInfo.FieldType;

        if (typeProperty.IsArray || typeProperty.GetInterface(nameof(IList)) != null)
        {
            SerializedProperty propArray = FindArray();
            EditorGUI.PropertyField(position, property, label);

            if (!Application.isPlaying && propArray.arraySize <= 1 && GUILayout.Button("GetComponentsInChildren"))
            {
                GetComponentsInChildrenAttribute attributeGCIC = attribute as GetComponentsInChildrenAttribute;
                typeProperty = typeProperty.IsArray ? typeProperty.GetElementType() : typeProperty.GetGenericArguments()[attributeGCIC.IndexGeneric];
                MonoBehaviour mono = property.serializedObject.targetObject as MonoBehaviour;
                SetPropertyArray(propArray, mono.gameObject.GetComponentsInChildren(typeProperty, attributeGCIC.IncludeInactive));
            }
        }
        else
        {
            EditorGUILayout.PropertyField(property, label);
            EditorGUILayout.HelpBox("Not array", UnityEditor.MessageType.Error);
        }


        #region Local: FindArray()
        //=================================
        SerializedProperty FindArray()
        {
            SerializedProperty iterator = property.serializedObject.GetIterator();
            while (iterator.name != fieldInfo.Name && iterator.Next(true)) ;

            return iterator;
        }
        //=================================
        void SetPropertyArray(SerializedProperty property, UnityEngine.Object[] array)
        {
            int count = array.Length;

            while (property.arraySize < count)
                property.InsertArrayElementAtIndex(property.arraySize);

            for (int index = 0; index < count; index++)
                property.GetArrayElementAtIndex(index).objectReferenceValue = array[index];

            while (property.arraySize > count)
                property.DeleteArrayElementAtIndex(count);
        }
        #endregion
    }
}
