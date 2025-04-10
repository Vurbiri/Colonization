//Assets\Vurbiri.UI\Editor\Editors\CmButtonEditor.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.UI;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VButton)), CanEditMultipleObjects]
    sealed public class VButtonEditor : VSelectableEditor
    {
        private const string NAME = "Button", RESOURCE = "VButton";
        private const string MENU = VUI_CONST_EDITOR.NAME_CREATE_MENU + NAME;

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

        [MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromPrefab(RESOURCE, NAME, command.context as GameObject);

    }
}
