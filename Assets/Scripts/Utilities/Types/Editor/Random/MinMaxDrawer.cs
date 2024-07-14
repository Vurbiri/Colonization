using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MinMax))]
public class MinMaxDrawer : PropertyDrawer
{
    private const float OFFSET_SIZE_LABEL = 20f, SIZE_VALUE = 85f, SIZE_SPACE = 5f;
    private const string NAME_MIN = "_min", NAME_MAX = "_max";


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;

        SerializedProperty minProperty = property.FindPropertyRelative(NAME_MIN);
        SerializedProperty maxProperty = property.FindPropertyRelative(NAME_MAX);

        Rect sizeLabel = position, sizeMin = position, sizeMax = position;

        sizeLabel.width = EditorGUIUtility.labelWidth + OFFSET_SIZE_LABEL;

        sizeMin.x = sizeLabel.width;
        sizeMax.x = sizeLabel.width + SIZE_VALUE + SIZE_SPACE;
        sizeMin.width = sizeMax.width = SIZE_VALUE;

        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.LabelField(sizeLabel, label);
        EditorGUI.PropertyField(sizeMin, minProperty, new GUIContent(""));
        EditorGUI.PropertyField(sizeMax, maxProperty, new GUIContent(""));

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;
}

