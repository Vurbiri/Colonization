using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace VurbiriEditor
{
    public abstract class AEditorGetVE<T> : Editor where T : AEditorGetVE<T>
    {
        private static Editor _self;

        protected VisualElement CreateDefaultInspectorGUI()
        {
            IMGUIContainer defaultInspector = new(() => DrawDefaultInspector());
            VisualElement root = new();
            root.Add(defaultInspector);
            return root;
        }

        public static VisualElement CreateEditorAndBind(Object obj, out Editor editor)
        {
            editor = CreateEditor(obj, typeof(T));
            VisualElement element = editor.CreateInspectorGUI();
            element.Bind(editor.serializedObject);
            return element;
        }

        public static VisualElement CreateCachedEditorAndBind(Object obj)
        {
            CreateCachedEditor(obj, typeof(T), ref _self);
            VisualElement element = _self.CreateInspectorGUI();
            element.Bind(_self.serializedObject);
            return element;
        }

        public static VisualElement CreateInstanceAndGetVisualElement() => CreateInstance<T>().CreateInspectorGUI();
    }
}
