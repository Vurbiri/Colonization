using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MinusPlusRange))]
public class MinusPlusRangeDrawer : PropertyDrawer
{
    private const string NAME_VALUE = "_value";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.width *= 0.575f;

        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(position, property.FindPropertyRelative(NAME_VALUE), label);
        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(ZeroRange))]
public class ZeroRangeDrawer : MinusPlusRangeDrawer
{

}
