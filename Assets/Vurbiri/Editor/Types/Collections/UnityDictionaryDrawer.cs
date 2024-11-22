//Assets\Vurbiri\Editor\Types\Collections\UnityDictionaryDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(UnityDictionary<,>))]
    public class UnityDictionaryDrawer : PropertyDrawer
    {
        private const string NAME_VALUE = "_values";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property.FindPropertyRelative(NAME_VALUE), label);
            EditorGUI.EndProperty();
        }
    }
}
