using UnityEditor;
using UnityEngine;
using Vurbiri.UI;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(CmButton), true), CanEditMultipleObjects]
    public class CmButtonEditor : CmSelectableEditor
    {
        private SerializedProperty m_OnClickProperty;

        protected override void OnEnable()
        {
            m_OnClickProperty = serializedObject.FindProperty("m_OnClick");
            base.OnEnable();
        }

        protected override void CustomEndPropertiesGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_OnClickProperty);
        }

        [MenuItem("GameObject/UI Custom/Button", false, 77)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromPrefab("CmButton", "Button", command.context as GameObject);

    }
}
