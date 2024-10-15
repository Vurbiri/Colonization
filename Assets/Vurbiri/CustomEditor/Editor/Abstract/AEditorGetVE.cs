using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace VurbiriEditor
{
    public abstract class AEditorGetVE<T> : Editor where T : AEditorGetVE<T>
    {
        public new VisualElement CreateInspectorGUI() => Create(serializedObject);

        protected abstract VisualElement Create(SerializedObject serializedObject);

        public static VisualElement BindAndGetVisualElement(SerializedObject serializedObject)
        {
            VisualElement element = CreateInstance<T>().Create(serializedObject);
            element.Bind(serializedObject);
            return element;
        }

        public static VisualElement GetVisualElement(SerializedObject serializedObject) => CreateInstance<T>().Create(serializedObject);
        public static VisualElement GetVisualElement() => CreateInstance<T>().Create(null);
    }
}
