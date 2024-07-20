using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReactiveValue<>))]
public class ReactiveValueDrawer : PropertyDrawer
{
    private const string NAME_VALUE = "_value";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(position, property.FindPropertyRelative(NAME_VALUE), label);
        EditorGUI.EndProperty();
    }
}
