using UnityEditor;
using UnityEngine;

namespace VurbiriEditor.ReColoringVertex
{
    using static CONST;

    [CustomEditor(typeof(PaletteVertexScriptable), true), CanEditMultipleObjects]
    internal class PaletteVertexScriptableEditor : Editor
    {
        public const string LABEL_IS_MODIFY = " Get VertexMaterials from mesh", LABEL_GET_BUTTON = "GET";
        public const string LABEL_NAME_BUTTON = "Names from colors", LABEL_CLEAR_BUTTON = "Clear names";

        protected SerializedProperty _isModifyProperty;
        protected SerializedProperty _materialFromMeshProperty;

        protected SerializedProperty _isInvertSubMeshesProperty;
        protected SerializedProperty _vertexMaterialsProperty;
        protected SerializedProperty _isEditNameProperty;

        protected PaletteVertexScriptable _palette;

        protected virtual void OnEnable()
        {
            _isModifyProperty = serializedObject.FindProperty("_isModify");
            _materialFromMeshProperty = serializedObject.FindProperty("_materialFromMesh");

            _isInvertSubMeshesProperty = serializedObject.FindProperty("_isInvertSubMeshes");
            _vertexMaterialsProperty = serializedObject.FindProperty("_vertexMaterials");
            _isEditNameProperty = serializedObject.FindProperty("_isEditName");

            _palette = (PaletteVertexScriptable)target;

            _materialFromMeshProperty.objectReferenceValue = null;
        }

        public override void OnInspectorGUI()
        {
            foreach (var m in _palette)
                m.Reset(_palette.IsEditName);

            serializedObject.Update();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUI.skin.window);
            EditorGUI.indentLevel++;
            if (_isModifyProperty.boolValue = EditorGUILayout.ToggleLeft(LABEL_IS_MODIFY, _isModifyProperty.boolValue))
            {
                EditorGUILayout.PropertyField(_materialFromMeshProperty);
                if (_palette.CheckMesh() && GUILayout.Button(LABEL_GET_BUTTON))
                    _palette.OpenWindow();
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(16);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(20);

            _isInvertSubMeshesProperty.boolValue = EditorGUILayout.Toggle(LABEL_IS_INVERT, _isInvertSubMeshesProperty.boolValue);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_vertexMaterialsProperty);
            EditorGUILayout.Space();
            _isEditNameProperty.boolValue = EditorGUILayout.ToggleLeft(LABEL_EDIT_NAMES, _isEditNameProperty.boolValue);
            EditorGUILayout.Space();
            if (GUILayout.Button(LABEL_NAME_BUTTON))
                _palette.NameFromColor();
            if (GUILayout.Button(LABEL_CLEAR_BUTTON))
                _palette.ClearNames();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
