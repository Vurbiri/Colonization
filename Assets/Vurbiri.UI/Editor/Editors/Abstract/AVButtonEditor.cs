//Assets\Vurbiri.UI\Editor\Editors\Abstract\AVButtonEditor.cs
using System.Collections.Generic;
using UnityEditor;
using Vurbiri.UI;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(AVButton), true), CanEditMultipleObjects]
    public abstract class AVButtonEditor : VSelectableEditor
    {
        private SerializedProperty _onClickProperty;

        protected override bool IsDerivedEditor => GetType() != typeof(AVButtonEditor);
        protected override HashSet<string> GetExcludePropertyPaths()
        {
            var exclude = base.GetExcludePropertyPaths();
            exclude.Add(_onClickProperty.propertyPath);
            return exclude;
        }

        protected override void OnEnable()
        {
            _onClickProperty = serializedObject.FindProperty("_onClick");
            base.OnEnable();
        }

        protected override void CustomEndPropertiesGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_onClickProperty);
            EditorGUILayout.Space();
        }
    }
}
