//Assets\VurbiriEditor.ReColoringVertex\Editor\Window\PaletteFromMeshWindow.cs
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VurbiriEditor.ReColoringVertex
{
    using static CONST_RCV;

    internal class PaletteFromMeshWindow : EditorWindow
    {
        public event Action<IEnumerable<VertexMaterial>> EventOverride;
        public event Action<IEnumerable<VertexMaterial>> EventAppend;

        private const string NAME = "Palette from mesh";
        private const string BUTTON_OVERRIDE = "Override", BUTTON_APPEND = "Append";
        private static readonly GUIContent NAME_CONTENT = new(NAME);

        private const int MAX_MATERIALS = 25;
        private const string KEY_X = "PFM_X", KEY_Y = "PFM_Y", KEY_W = "PFM_Width", KEY_H = "PFM_Height";


        private List<ExportMaterial> _materials;
        private Vector2 _scrollPos;
        private string _nameMesh;
        private GUIStyle _styleMesh;

        #region OnEnable/OnDisable
        private void OnEnable()
        {
            _styleMesh = new(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };

            minSize = wndMinSize;
            titleContent = NAME_CONTENT;

            if (EditorPrefs.HasKey(KEY_X) && EditorPrefs.HasKey(KEY_Y) && EditorPrefs.HasKey(KEY_W) && EditorPrefs.HasKey(KEY_H))
                position = new(EditorPrefs.GetFloat(KEY_X), EditorPrefs.GetFloat(KEY_Y), EditorPrefs.GetFloat(KEY_W), EditorPrefs.GetFloat(KEY_H));
        }

        private void OnDisable()
        {
            EditorPrefs.SetFloat(KEY_X, position.x);
            EditorPrefs.SetFloat(KEY_Y, position.y);
            EditorPrefs.SetFloat(KEY_W, position.width);
            EditorPrefs.SetFloat(KEY_H, position.height);

            EventOverride = null;
            EventAppend = null;
        }
        #endregion

        private void OnGUI()
        {
            if (_materials == null || Application.isPlaying) 
                return;

            BeginWindows();

            EditorGUILayout.Space(SPACE_WND);
            EditorGUILayout.LabelField(_nameMesh, _styleMesh);
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUI.skin.window);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            foreach (var mat in _materials)
                mat.Draw();

            EditorGUILayout.Space(16);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            if (GUILayout.Button(BUTTON_OVERRIDE))
                ButtonClick(EventOverride);
            EditorGUILayout.Space();
            if (GUILayout.Button(BUTTON_APPEND))
                ButtonClick(EventAppend);

            EditorGUILayout.Space(SPACE_WND);
            EndWindows();

            #region Local: ButtonClick(...)
            //=================================
            void ButtonClick(Action<IEnumerable<VertexMaterial>> action)
            {
                if (_materials != null)
                    action?.Invoke(_materials.Where(m => m.enable).Select(m => m.material));

                Close();
                _materials = null;
            }
            #endregion
        }

        public void CreateColorsData(Mesh mesh)
        {
            if (mesh == null)
            {
                Debug.LogError("Mesh NULL");
                Close();
                return;
            }

            Color[] colors = mesh.colors;
            Vector2[] uvs = mesh.uv;

            int vertexCount = colors.Length;

            if (vertexCount <= 0 || vertexCount != uvs.Length)
            {
                Debug.LogError("Mesh Error");
                Close();
                return;
            }


            _materials = new();
            _nameMesh = mesh.name;

            Color currentColor;
            Vector2 currentUV;

            for (int vertex = 0; vertex < vertexCount; vertex++)
            {
                
                currentColor = colors[vertex];
                currentUV = uvs[vertex];

                if (FindMaterial())
                    continue;

                _materials.Add(new(currentColor, currentUV, _materials.Count + 1));
                
                if(_materials.Count > MAX_MATERIALS)
                {
                    _materials = null;
                    Debug.LogError("There are too many materials");
                    Close();
                    return;
                }
            }

            #region Local: FindMaterial()
            //=================================
            bool FindMaterial()
            {
                foreach (var mat in _materials)
                    if (mat.Equals(currentColor))
                        return true;
                return false;
            }
            #endregion
        }

        #region Nested: ExportMaterial
        //***********************************
        private class ExportMaterial
        {
            public bool enable;
            public VertexMaterial material;

            private Color _colorGUI;
            private GUIContent labelEnable = new("Enable");

            public ExportMaterial(Color colorReplace, Vector2 specular, int id)
            {
                enable = true;
                material = new(colorReplace, specular);

                labelEnable = new($" {id}");
                _colorGUI = GUI.color;
            }

            public bool Equals(Color colorReplace) => material.colorReplace == colorReplace;

            public void Draw()
            {
                EditorGUILayout.BeginVertical(GUI.skin.window);

                if (!(enable = EditorGUILayout.ToggleLeft(labelEnable, enable)))
                    GUI.color = Color.gray;

                EditorGUILayout.Space();
                material.colorReplace = EditorGUILayout.ColorField(labelColor, material.colorReplace, true, true, false);
                for (int i = 0; i < COUNT_SPECULAR; i++)
                    material.specular[i] = EditorGUILayout.Slider(labelsSpecular[i], material.specular[i], 0f, 1f);

                GUI.color = _colorGUI;

                EditorGUILayout.Space(16);
                EditorGUILayout.EndVertical();
                
            }

            public static implicit operator VertexMaterial(ExportMaterial value) => value.material;

        }
        #endregion
    }
}
