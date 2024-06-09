using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RenameAttribute))]
public class RenameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label.text = ((RenameAttribute)attribute).Name;

        EditorGUI.PropertyField(position, property, label);
    }
}
