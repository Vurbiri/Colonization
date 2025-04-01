//Assets\Vurbiri.UI\Editor\Editors\VToggleEditor.cs
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Vurbiri.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VToggle)), CanEditMultipleObjects]
    public class VToggleEditor : VSelectableEditor
    {
        private SerializedProperty _onValueChangedProperty;
        private SerializedProperty _transitionProperty;
        private SerializedProperty _markOnGraphicProperty;
        private SerializedProperty _groupProperty;
        private SerializedProperty _isOnProperty;

        private VToggle _toggle;

        protected override void OnEnable()
        {
            _toggle = target as VToggle;

            _transitionProperty = serializedObject.FindProperty("_toggleTransition");
            _markOnGraphicProperty = serializedObject.FindProperty("_markOnGraphic");
            _groupProperty = serializedObject.FindProperty("_group");
            _isOnProperty = serializedObject.FindProperty("_isOn");
            _onValueChangedProperty = serializedObject.FindProperty("_onValueChanged");

            base.OnEnable();
        }

        protected override void CustomMiddlePropertiesGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            PropertyField(_isOnProperty);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(_toggle.gameObject.scene);

                VToggleGroup group = _toggle.group;

                if (group != null && _toggle.isActiveAndEnabled)
                    _isOnProperty.boolValue = group.CheckValue_Editor(_toggle, _isOnProperty.boolValue);

                _toggle.isOn = _isOnProperty.boolValue;
            }
            PropertyField(_transitionProperty);
            PropertyField(_markOnGraphicProperty);

            Space();

            EditorGUI.BeginChangeCheck();
            PropertyField(_groupProperty);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(_toggle.gameObject.scene);

                _toggle.group = _groupProperty.objectReferenceValue as VToggleGroup;
            }

            Space();

            serializedObject.ApplyModifiedProperties();
        }

        protected override void CustomEndPropertiesGUI()
        {
            Space();
            PropertyField(_onValueChangedProperty);
        }
    }
}
