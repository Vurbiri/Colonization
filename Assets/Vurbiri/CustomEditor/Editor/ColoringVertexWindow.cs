using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vurbiri
{
    public class ColoringVertexWindow : EditorWindow
    {
        #region string consts
        private const string NAME = "ReColoring Vertex", MENU = "Window/Vurbiri/" + NAME;
        private const string LABEL_PALETTE = "Palette", LABEL_MESH = "Mesh", LABEL_IS_SAVE = "Save palette for mesh";
        private const string LABEL_PALETTE_PATH = "Path save palette", LABEL_MESH_PATH = "Save path", LABEL_MESH_NAME = "Name";
        private const string LABEL_IS_INVERT = "Invert SubMesh";
        private const string LABEL_MESH_BUTTON = "Save mesh as...", LABEL_RELOAD_BUTTON = "Reload";
        private const string LABEL_COLOR = "Mesh Color", LABEL_REPLACE = "Replace Color";
        private const string KEY_ISSAVE = "CV_IsSave", KEY_MESH_PATH = "CV_MeshPath";
        private const string PALETTE_PATH = "Assets/Vurbiri/CustomEditor/Editor/Resources/";
        private const string PALETTE_EXP = ".asset", MESH_EXP = ".mesh";
        private const string DEFOULT_PALETTE = "Palette_Default", PREFIX_PALETTE = "Palette_", PREFIX_MESH = "MH_";
        #endregion

        #region Varibles
        private Mesh _mesh, _tempMesh;
        private PaletteVertexScriptable _palette, _tempPalette;
        private readonly GUIContent _labelColor = new(LABEL_COLOR);
        private Vector2 _scrollPos;

        private bool _isSavePalette = true, _isInvert = false;
        private string _savePath = "Assets/", _name;

        private readonly Type _typeMesh = typeof(Mesh), _typePalette = typeof(PaletteVertexScriptable);

        private int _arrayColorsCount, _subMeshCount, _colorsCount;
        private List<ColorData>[] _colorData;
        #endregion

        private string NamePaletteFromMesh => PREFIX_PALETTE + _mesh.name;

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<ColoringVertexWindow>(true, NAME);
        }

        #region OnEnable/OnDisable
        private void OnEnable()
        {
            if (EditorPrefs.HasKey(KEY_ISSAVE))
                _isSavePalette = EditorPrefs.GetBool(KEY_ISSAVE);

            if (EditorPrefs.HasKey(KEY_MESH_PATH))
                _savePath = EditorPrefs.GetString(KEY_MESH_PATH);

            _palette = Resources.Load<PaletteVertexScriptable>(DEFOULT_PALETTE);
        }

        private void OnDisable()
        {
            EditorPrefs.SetBool(KEY_ISSAVE, _isSavePalette);
            EditorPrefs.SetString(KEY_MESH_PATH, _savePath);
        }
        #endregion

        private void OnGUI()
        {
            EditorGUILayout.Space(20);
            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("NOT EDIT: ", "PLAYING", EditorStyles.boldLabel);
                return;
            }

            if (GUILayout.Button(LABEL_RELOAD_BUTTON))
                Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(LABEL_PALETTE_PATH, PALETTE_PATH, EditorStyles.boldLabel);
            _tempPalette = EditorGUILayout.ObjectField(LABEL_PALETTE, _palette, _typePalette, false) as PaletteVertexScriptable;
            _tempMesh = EditorGUILayout.ObjectField(LABEL_MESH, _mesh, _typeMesh, false) as Mesh;

            if (_tempPalette != _palette)
            {
                _palette = _tempPalette;
                 SetPalette();
            }

            if (_tempMesh != _mesh)
            {
                _mesh = _tempMesh;
                Update();
            }

            if (_mesh == null || _palette == null || _colorData == null)
                return;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUI.indentLevel++;
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            for (int i = 0; i < _colorData.Length; i++)
                DrawSubMesh(_colorData[i], i);

            EditorGUILayout.EndScrollView();
            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.Space();
            _isSavePalette = EditorGUILayout.Toggle(LABEL_IS_SAVE, _isSavePalette);
            _isInvert = EditorGUILayout.Toggle(LABEL_IS_INVERT, _isInvert);

            EditorGUILayout.Space();
            _savePath = EditorGUILayout.TextField(LABEL_MESH_PATH, _savePath);
            _name = EditorGUILayout.TextField(LABEL_MESH_NAME, _name);

            EditorGUILayout.Space();
            if (GUILayout.Button(LABEL_MESH_BUTTON))
                SaveMesh();

            EditorGUILayout.Space(20);

            #region Local: Update(), DrawSubMesh, DrawColorData(...), FindPallete()
            //=================================
            void Update()
            {
                if (_mesh != null)
                {
                    _subMeshCount = _mesh.subMeshCount;
                    FindPalette(NamePaletteFromMesh);
                    CreateColorsData();
                }
            }
            //=================================
            void DrawSubMesh(List<ColorData> currentData, int id)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField($"Sub Mesh: {id}", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical("box");
                for (int i = 0; i < currentData.Count; i++)
                    DrawColorData(currentData[i]);
                EditorGUILayout.EndVertical();
            }
            //=================================
            void DrawColorData(ColorData data)
            {
                EditorGUILayout.Space(2);
                EditorGUILayout.LabelField(data.id.ToString(), EditorStyles.boldLabel);
                EditorGUILayout.ColorField(_labelColor, data.colorMesh, false, false, false);
                data.colorReplace = EditorGUILayout.ColorField(LABEL_REPLACE, data.colorReplace);
                EditorGUILayout.Space(2);
            }
            //=================================
            void FindPalette(string name)
            {
                foreach(var palette in Resources.FindObjectsOfTypeAll<PaletteVertexScriptable>())
                {
                    if (palette.name == name)
                    {
                        _palette = palette;
                        return;
                    }
                }
            }
            #endregion
        }

        private void CreateColorsData()
        {
            if (_mesh == null || _palette == null)
                return;

            _name = PREFIX_MESH + _mesh.name;

            Color[] colors = _mesh.colors;
            _arrayColorsCount = colors.Length;

            HashSet<int>[] subMeshes = new HashSet<int>[_subMeshCount];
            _colorData = new List<ColorData>[_subMeshCount];

            for (int i = 0; i < _subMeshCount; i++)
            {
                _colorData[i] = new();
                subMeshes[i] = new(_mesh.GetTriangles(i));
            }

            int index;
            Color32 currentColor;
            List<ColorData> currentData = null;
            _colorsCount = 0;
            for (int i = 0; i < _arrayColorsCount; i++)
            {
                for (int j = 0; j < _subMeshCount; j++)
                {
                    if(subMeshes[j].Contains(i))
                    {
                        currentData = _colorData[j];
                        break;
                    }
                }

                currentColor = colors[i];
                index = currentData.FindIndex(cd => cd.colorMesh == currentColor);
                if (index == -1)
                {
                    if (_palette.Count <= _colorsCount)
                        currentData.Add(new(currentColor, Color.white, currentData.Count, i));
                    else
                        currentData.Add(new(currentColor, _palette[_colorsCount], currentData.Count, i));

                    _colorsCount++;
                }
                else
                {
                    currentData[index].Add(i);
                }
            }
        }

        private void SetPalette()
        {
            if (_mesh == null || _palette == null || _colorData == null)
                return;

            Debug.Log("SetPalette");

            int colorsCount = 0;
            List<ColorData> currentData;
            for (int i = 0; i < _subMeshCount; i++)
            {
                currentData = _colorData[i];
                if (currentData == null)
                    return;

                for (int j = 0; j < currentData.Count; j++)
                {
                    if (_palette.Count <= colorsCount)
                        return;

                    currentData[j].colorReplace = _palette[colorsCount];
                    colorsCount++;
                }

            }
        }

        private void SaveMesh()
        {
            if (_isSavePalette)
                SavePalette();

            Mesh newMesh = NewMesh();
            Color32[] colors = new Color32[_arrayColorsCount];
            Color32 color;

            foreach (var lisData in _colorData)
            {
                foreach (var data in lisData)
                {
                    color = data.colorReplace;
                    foreach (int i in data.indexesVertex)
                        colors[i] = color;
                }
            }

            newMesh.colors32 = colors;
            newMesh.Optimize();
            newMesh.UploadMeshData(true);

            string path = _savePath + _name + MESH_EXP;
            AssetDatabase.CreateAsset(newMesh, path);
            
            Debug.Log($"Mesh save: {path}");

            //AssetDatabase.SaveAssets();

            //EditorUtility.FocusProjectWindow();
            //Selection.activeObject = newMesh;

            #region Local: SavePalette(), NewMesh(), NamePaletteFromMesh()
            //=================================
            void SavePalette()
            {
                _palette = CreateInstance<PaletteVertexScriptable>();
                _palette.Initialize(NamePaletteFromMesh, _colorsCount);

                foreach (var lisData in _colorData)
                    foreach (var data in lisData)
                        _palette.Add(data.colorReplace);

                string path = PALETTE_PATH + _palette.name + PALETTE_EXP;
                AssetDatabase.CreateAsset(_palette, path);
                
                Debug.Log($"Palette save: {path}");
            }
            //=================================
            Mesh NewMesh()
            {
                Mesh mesh = new()
                {
                    name = _name,
                    vertices = _mesh.vertices,
                    normals = _mesh.normals,
                    bounds = _mesh.bounds,
                    tangents = _mesh.tangents ?? null,
                    uv = _mesh.uv ?? null,
                };

                int count = mesh.subMeshCount = _subMeshCount, increment = _isInvert ? -1 : 1;
                for (int i = 0, j = _isInvert ? count - 1 : 0 ; i < count; i++, j += increment)
                    mesh.SetTriangles(_mesh.GetTriangles(i), j);

                if(_isInvert)
                    mesh.RecalculateBounds();

                return mesh;
            }
            #endregion
        }


        #region Nested: InitializeOnLoad, Postprocessor
        //*******************************************************
        private class ColorData
        {
            public Color colorMesh;
            public Color colorReplace;
            public int id;
            public List<int> indexesVertex;

            public ColorData(Color color, Color replace, int id, int index)
            {
                colorMesh = color;
                colorReplace = replace;
                this.id = id;
                indexesVertex = new()
                {
                    index
                };
            }

            public void Add(int index) => indexesVertex.Add(index);
        }
        #endregion
    }
}
