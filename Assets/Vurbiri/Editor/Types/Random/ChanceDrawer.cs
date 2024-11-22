//Assets\Vurbiri\Editor\Types\Random\ChanceDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(Chance))]
    public class ChanceDrawer : PropertyDrawer
    {
        private const string NAME_VALUE = "_value", NAME_ENT = "_negentropy";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative(NAME_VALUE);
            property.FindPropertyRelative(NAME_ENT).intValue = Random.Range(0, 100);

            label = EditorGUI.BeginProperty(position, label, property);
            valueProperty.intValue = EditorGUI.IntSlider(position, label, valueProperty.intValue, 0, 100);
            EditorGUI.EndProperty();
        }
    }
}
