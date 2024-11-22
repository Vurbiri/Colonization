//Assets\Vurbiri\Editor\ReColoringVertex\Window\PaletteWindow.cs
using UnityEditor;
using UnityEngine;

namespace VurbiriEditor.ReColoringVertex
{
    using static CONST_RCV;

    [EditorWindowTitle(icon = "Assets/Vurbiri/ReColoringVertex/Editor/Icons/IconEditorPalette.png", title = NAME)]
    internal class PaletteWindow : EditorWindow
    {
        private const string NAME_PROPERTY = "_vertexMaterials";
        private const string KEY_X = "P_X", KEY_Y = "P_Y", KEY_W = "P_Width", KEY_H = "P_Height";
        private const string NAME = "Palette Vertex";
        private const string LABEL_PALETTE = "Palette";

        private Vector2 _scrollPos;

        private PaletteVertexScriptable _reference;
        private SerializedProperty _property;
        private SerializedObject _palette;

        public void SetPalette(PaletteVertexScriptable palette)
        {
            if (palette == _reference)
                return;

            _reference = palette;
            SetProperty();
        }


        #region OnEnable/OnDisable
        private void OnEnable()
        {
            SetProperty();

            minSize = wndMinSize;

            if (EditorPrefs.HasKey(KEY_X) && EditorPrefs.HasKey(KEY_Y) && EditorPrefs.HasKey(KEY_W) && EditorPrefs.HasKey(KEY_H))
                position = new(EditorPrefs.GetFloat(KEY_X), EditorPrefs.GetFloat(KEY_Y), EditorPrefs.GetFloat(KEY_W), EditorPrefs.GetFloat(KEY_H));
        }

        private void OnDisable()
        {
            EditorPrefs.SetFloat(KEY_X, position.x);
            EditorPrefs.SetFloat(KEY_Y, position.y);
            EditorPrefs.SetFloat(KEY_W, position.width);
            EditorPrefs.SetFloat(KEY_H, position.height);
        }
        #endregion

        private void OnGUI()
        {
            BeginWindows();

            EditorGUILayout.Space(SPACE_WND);
            SetPalette(EditorGUILayout.ObjectField(LABEL_PALETTE, _reference, typePalette, false) as PaletteVertexScriptable);

            if (_property != null)
            {
                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical();
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                EditorGUILayout.PropertyField(_property);

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }

            EndWindows();
        }

        private void SetProperty()
        {
            if(_reference == null)
            {
                _palette = null;
                _property = null;
                return;
            }

            _reference.IsEditName = false;
            _reference.IsOpen = true;

            _palette = new(_reference);
            _property = _palette?.FindProperty(NAME_PROPERTY);
            _palette.Update();
        }
    }
}
