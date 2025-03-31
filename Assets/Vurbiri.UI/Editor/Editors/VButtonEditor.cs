//Assets\Vurbiri.UI\Editor\Editors\CmButtonEditor.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.UI;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VButton), true), CanEditMultipleObjects]
    public class VButtonEditor : VSelectableEditor
    {
        private SerializedProperty _onClickProperty;

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

        [MenuItem("GameObject/UI Vurbiri/Button", false, 77)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromPrefab("VButton", "Button", command.context as GameObject);

    }
}
