using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
    public abstract class ADirectionDrawer<T> : PropertyDrawer
    {
        protected const string NAME_VALUE = "_dir";

        protected abstract string[] Names { get; }
        protected abstract int[] Indexes { get; }
        protected abstract T[] Values { get; }

        protected T Drawer(Rect position, SerializedProperty property, GUIContent label, T value)
        {
            int index;

            for (index = Values.Length - 1; index >= 0; index--)
                if (value.Equals(Values[index]))
                    break;

            label = EditorGUI.BeginProperty(position, label, property);
            index = EditorGUI.IntPopup(position, label.text, index, Names, Indexes, EditorStyles.popup);
            EditorGUI.EndProperty();

            return Values[index];
        }
    }
}
