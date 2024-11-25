//Assets\VurbiriEditor.ReColoringVertex\Editor\Window\ReColoringVertexWindow.cs
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace VurbiriEditor.ReColoringVertex
{
    using static CONST_RCV;

    internal class ReColoringVertexWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "ReColoring Vertex", MENU = MENU_PATH + NAME, C_MENU = "Assets/" + NAME;
        private const string LABEL_PALETTE = "Palette", LABEL_MESH = "Mesh", LABEL_IS_SAVE = "Save palette for mesh";
        private const string LABEL_PALETTE_PATH = "Path to palettes:";
        private const string LABEL_MESH_BUTTON = "Save mesh as...", LABEL_RELOAD_BUTTON = "Reload";

        private const string PREFIX_PALETTE = "Palette_", PREFIX_MESH = "MH_";

        private const string KEY_IS_SAVE = "CV_IsSave", KEY_MESH_PATH = "CV_MeshPath", KEY_EDIT_NAMES = "CV_EditNames";
        private const string KEY_X = "CV_X", KEY_Y = "CV_Y", KEY_W = "CV_Width", KEY_H = "CV_Height";
        #endregion

        #region Varibles
        private Mesh _currentMesh;
        private PaletteVertexScriptable _currentPalette;

        private bool _isInvert = false;
        private string _nameMesh;

        private int _vertexCount, _subMeshCount, _colorsCount;
        private ListVertexMaterials[] _listData;

        private Vector2 _scrollPos;
        private bool _isSavePalette = true, _isEditName = false, _isEditedMesh = false;
        private string _saveFolder = "Assets/";
        #endregion

        private string NamePaletteFromMesh => string.Concat(PREFIX_PALETTE, _currentMesh.name);

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<ReColoringVertexWindow>(true, NAME).minSize = wndMinSize;
        }

        [MenuItem(C_MENU, false, 71)]
        private static void ShowContextWindow()
        {
            if(Selection.activeObject is Mesh mesh)
            {
                var wnd = GetWindow<ReColoringVertexWindow>(true, NAME);
                wnd.minSize = wndMinSize;
                wnd.CreateColorsData(mesh);
            }
        }

        [MenuItem(C_MENU, true, 71)]
        private static bool CheckContextMenu() => Selection.activeObject is Mesh;

        #region OnEnable/OnDisable
        private void OnEnable()
        {
            if (EditorPrefs.HasKey(KEY_EDIT_NAMES))
                _isEditName = EditorPrefs.GetBool(KEY_EDIT_NAMES);

            if (EditorPrefs.HasKey(KEY_IS_SAVE))
                _isSavePalette = EditorPrefs.GetBool(KEY_IS_SAVE);

            if (EditorPrefs.HasKey(KEY_MESH_PATH))
                _saveFolder = EditorPrefs.GetString(KEY_MESH_PATH);

            if (EditorPrefs.HasKey(KEY_X) && EditorPrefs.HasKey(KEY_Y) && EditorPrefs.HasKey(KEY_W) && EditorPrefs.HasKey(KEY_H))
                position = new(EditorPrefs.GetFloat(KEY_X), EditorPrefs.GetFloat(KEY_Y), EditorPrefs.GetFloat(KEY_W), EditorPrefs.GetFloat(KEY_H));
        }

        private void OnDisable()
        {
            EditorPrefs.SetBool(KEY_EDIT_NAMES, _isEditName);
            EditorPrefs.SetBool(KEY_IS_SAVE, _isSavePalette);
            EditorPrefs.SetString(KEY_MESH_PATH, _saveFolder);
            EditorPrefs.SetFloat(KEY_X, position.x);
            EditorPrefs.SetFloat(KEY_Y, position.y);
            EditorPrefs.SetFloat(KEY_W, position.width);
            EditorPrefs.SetFloat(KEY_H, position.height);
        }
        #endregion

        private void OnGUI()
        {
            if (Application.isPlaying)
            {
                Close();
                return;
            }

            BeginWindows();

            DrawTop();

            if (_currentMesh == null || _listData == null)
            {
                EndWindows();
                return;
            }

            DrawColorsMesh();

            DrawBottom();

            EndWindows();

            #region Local: DrawTop(), DrawColorsMesh(),  DrawBottom()
            //=================================
            void DrawTop()
            {
                EditorGUILayout.Space(SPACE_WND);

                EditorGUILayout.LabelField(LABEL_PALETTE_PATH, PALETTE_PATH, EditorStyles.boldLabel);
                SetPalette(EditorGUILayout.ObjectField(LABEL_PALETTE, _currentPalette, typePalette, false) as PaletteVertexScriptable);
                if (GUILayout.Button("Show palette"))
                {
                    var wnd = GetWindow<PaletteWindow>();
                    wnd.SetPalette(_currentPalette);
                    FocusWindowIfItsOpen<ReColoringVertexWindow>();
                }
                    
                EditorGUILayout.Space();
                CreateColorsData(EditorGUILayout.ObjectField(LABEL_MESH, _currentMesh, typeMesh, false) as Mesh);

                EditorGUILayout.Space();
            }
            //=================================
            void DrawColorsMesh()
            {
                _isEditName = EditorGUILayout.ToggleLeft(LABEL_EDIT_NAMES, _isEditName);
                if (GUILayout.Button(LABEL_RELOAD_BUTTON))
                    CreateColorsData(_currentMesh, true);
                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical(GUI.skin.window);
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                foreach (var data in _listData)
                    data.Draw(_isEditName);

                EditorGUILayout.Space(16);
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }
            //=================================
            void DrawBottom()
            {
                EditorGUILayout.Space();

                _isSavePalette = EditorGUILayout.ToggleLeft(LABEL_IS_SAVE, _isSavePalette);

                if (_subMeshCount > 1)
                {
                    Color prevColor = GUI.color;
                    GUI.color = _isInvert ? Color.red : Color.green;
                    _isInvert = EditorGUILayout.ToggleLeft(LABEL_IS_INVERT, _isInvert);
                    GUI.color = prevColor;
                }

                EditorGUILayout.Space();
                if (GUILayout.Button(LABEL_MESH_BUTTON))
                    SaveMesh();

                EditorGUILayout.Space(SPACE_WND);
            }
            #endregion
        }

        public void CreateColorsData(Mesh mesh, bool isReload = false)
        {
            if (!isReload && _currentMesh == mesh)
                return;

            _currentMesh = mesh;

            if (_currentMesh == null)
                return;

            FindPalette(NamePaletteFromMesh);
            Selection.activeObject = _currentMesh;

            _subMeshCount = _currentMesh.subMeshCount;
            _nameMesh = _currentMesh.name;

            if(!(_isEditedMesh = _nameMesh.StartsWith(PREFIX_MESH)))
                _nameMesh = string.Concat(PREFIX_MESH, _nameMesh);

            Color[] colors = _currentMesh.colors;
            Vector2[] uvs = _currentMesh.uv;
            _vertexCount = colors.Length;

            if (_vertexCount == 0 || _vertexCount != uvs.Length)
            {
                _listData = null;
                return;
            }


            HashSet<int>[] subMeshes = new HashSet<int>[_subMeshCount];
            _listData = new ListVertexMaterials[_subMeshCount];

            for (int i = 0; i < _subMeshCount; i++)
            {
                _listData[i] = CreateInstance<ListVertexMaterials>().Init(i);
                subMeshes[i] = new(_currentMesh.GetTriangles(i));
            }

            Color currentColor;
            ListVertexMaterials listVert = null;
            _colorsCount = 0;
            for (int vertex = 0; vertex < _vertexCount; vertex++)
            {
                for (int j = 0; j < _subMeshCount; j++)
                {
                    if (subMeshes[j].Contains(vertex))
                    {
                        listVert = _listData[j];
                        break;
                    }
                }

                currentColor = colors[vertex];

                if (listVert.TryInsert(currentColor, vertex))
                    continue;

                if (_currentPalette != null && _currentPalette.Count > _colorsCount)
                    listVert.Add(currentColor, _currentPalette[_colorsCount], vertex);
                else if(_isEditedMesh)
                    listVert.Add(currentColor, uvs[vertex], vertex);
                else
                    listVert.Add(currentColor, vertex);

                _colorsCount++;
            }

            _isInvert = _subMeshCount > 1 && (_currentPalette != null && _currentPalette.IsInvert || _listData[0].Count > 1);

            #region Local: FindPalette(...)
            //=================================
            void FindPalette(string name)
            {
                var palette = Resources.Load<PaletteVertexScriptable>(name);

                if (palette != null)
                    _currentPalette = palette;
            }
            #endregion
        }

        private void SetPalette(PaletteVertexScriptable plt)
        {
            if(plt == _currentPalette)
                return;

            _currentPalette = plt;
            
            if (_currentMesh == null || _currentPalette == null || _listData == null)
                return;

            _isInvert = _currentPalette.IsInvert;

            int colorsCount = 0;
            ListVertexMaterials listVertexes;
            for (int i = 0; i < _subMeshCount; i++)
            {
                listVertexes = _listData[i];
                if (listVertexes == null)
                    return;

                for (int j = 0; j < listVertexes.Count; j++)
                {
                    if (_currentPalette.Count <= colorsCount)
                        return;

                    listVertexes[j].Material = _currentPalette[colorsCount];
                    colorsCount++;
                }
            }
        }

        private void SaveMesh()
        {
            if (!SetPath(out string path))
                return;

            if (_isSavePalette)
                SavePalette();

            Mesh newMesh = CopyMesh();
            Color32[] colors = new Color32[_vertexCount];
            Vector2[] uvs = new Vector2[_vertexCount];

            foreach (var lisData in _listData)
            {
                if (lisData.IsEdit)
                    lisData.CopyValues(colors, uvs);
                else
                    lisData.CopyValues(colors, uvs, _currentMesh.colors32, _currentMesh.uv);
            }

            newMesh.colors32 = colors;
            newMesh.uv = uvs;
            newMesh.Optimize();
            newMesh.UploadMeshData(true);

            AssetDatabase.CreateAsset(newMesh, path);
            AssetDatabase.SaveAssets();

            Debug.Log($"Mesh is saved: {path}");

            FocusWindowIfItsOpen<ReColoringVertexWindow>();
            //EditorUtility.FocusProjectWindow();
            //Selection.activeObject = newMesh;

            #region Local: SetPath(), SavePalette(), CopyMesh()
            //=================================
            bool SetPath(out string path)
            {
                path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save mesh", _nameMesh, MESH_EXP, "", _saveFolder);

                if (string.IsNullOrEmpty(path))
                    return false;

                _nameMesh = Path.GetFileNameWithoutExtension(path);
                _saveFolder = Path.GetDirectoryName(path);

                return true;
            }
            //=================================
            void SavePalette()
            {
                _currentPalette = CreateInstance<PaletteVertexScriptable>();
                _currentPalette.Init(NamePaletteFromMesh, _isInvert, _isEditName, _colorsCount);

                foreach (var lisData in _listData)
                    foreach (var data in lisData)
                        _currentPalette.Add(data.Material);

                string path = string.Concat(PALETTE_PATH, _currentPalette.name, ".", PALETTE_EXP);
                AssetDatabase.CreateAsset(_currentPalette, path);

                Debug.Log($"Palette is saved: {path}");
            }
            //=================================
            Mesh CopyMesh()
            {
                Mesh mesh = new()
                {
                    name = _nameMesh,
                    vertices = _currentMesh.vertices,
                    normals = _currentMesh.normals,
                    bounds = _currentMesh.bounds,
                    tangents = _currentMesh.tangents ?? null,
                    uv2 = _currentMesh.uv2 ?? null,
                };

                int count = mesh.subMeshCount = _subMeshCount, increment = _isInvert ? -1 : 1;
                for (int i = 0, j = _isInvert ? count - 1 : 0; i < count; i++, j += increment)
                    mesh.SetTriangles(_currentMesh.GetTriangles(i), j);

                if (_isInvert)
                    mesh.RecalculateBounds();

                return mesh;
            }
            #endregion
        }
    }
}
