using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(Chance))]
    public class ChanceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative(Chance.valueField);
            SerializedProperty negentropyProperty = property.FindPropertyRelative(Chance.negentropyField);

            if (negentropyProperty.intValue == 0)
                negentropyProperty.intValue = Random.Range(1, Chance.MAX_CHANCE);

            label = EditorGUI.BeginProperty(position, label, property);
            valueProperty.intValue = EditorGUI.IntSlider(position, label, valueProperty.intValue, 0, Chance.MAX_CHANCE);
            EditorGUI.EndProperty();
        }
    }
}
