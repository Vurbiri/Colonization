using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomPropertyDrawer(typeof(Currencies.CurrencyMain))]
    public class CurrencyDrawer : PropertyDrawer
    {
        private const string NAME_VALUE = "_value";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative(NAME_VALUE);

            label = EditorGUI.BeginProperty(position, label, property);
            valueProperty.intValue = EditorGUI.IntSlider(position, label, valueProperty.intValue, 0, 20);
            EditorGUI.EndProperty();
        }
    }
}
