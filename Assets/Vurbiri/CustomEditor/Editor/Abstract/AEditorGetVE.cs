using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace VurbiriEditors
{
    public abstract class AEditorGetVE<T> : Editor where T : Editor
    {
        public static VisualElement GetVisualElement(SerializedObject serializedObject)
        {
            VisualElement element = CreateInstance<T>().CreateInspectorGUI();
            element.Bind(serializedObject);
            return element;
        }

        public static VisualElement GetVisualElement() => CreateInstance<T>().CreateInspectorGUI();
    }
}
