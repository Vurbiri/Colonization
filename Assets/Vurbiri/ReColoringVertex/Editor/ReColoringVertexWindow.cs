using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor.ReColoringVertex
{
    using static CONST;

    internal class ReColoringVertexWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "ReColoring Vertex", MENU = MENU_PATH + NAME;
        private const string LABEL_PALETTE = "Palette", LABEL_MESH = "Mesh", LABEL_IS_SAVE = "Save palette for mesh";
        private const string LABEL_PALETTE_PATH = "Path to palettes:";
        private const string LABEL_MESH_BUTTON = "Save mesh as...", LABEL_RELOAD_BUTTON = "Reload";

        private const string DEFOULT_PALETTE = "Palette_Default", PREFIX_PALETTE = "Palette_", PREFIX_MESH = "MH_";

        private const string KEY_IS_SAVE = "CV_IsSave", KEY_MESH_PATH = "CV_MeshPath", KEY_EDIT_NAMES = "CV_EditNames";
        private const string KEY_X = "CV_X", KEY_Y = "CV_Y", KEY_W = "CV_Width", KEY_H = "CV_Height";

        #endregion

        #region Varibles
        private static Mesh currentMesh;
        private static PaletteVertexScriptable currentPalette;

        private static bool isInvert = false;
        private static string nameMesh;

        private static int vertexCount, subMeshCount, colorsCount;
        private static ListVertexMaterials[] listData;

        private Mesh _tempMesh;
        private PaletteVertexScriptable _tempPalette;
        private Vector2 _scrollPos;
        private bool _isSavePalette = true, _isEditName = false;
        private string _saveFolder = "Assets/";
        #endregion

        private string NamePaletteFromMesh => PREFIX_PALETTE.Concat(currentMesh.name);

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<ReColoringVertexWindow>(true, NAME).minSize = wndMinSize;
        }

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

            if (currentPalette == null)
                currentPalette = Resources.Load<PaletteVertexScriptable>(DEFOULT_PALETTE);
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
            if (Application.isPlaying) return;

            BeginWindows();

            DrawTop();

            if (_tempPalette != currentPalette)
            {
                currentPalette = _tempPalette;
                SetPalette();
            }
            if (_tempMesh != currentMesh)
            {
                currentMesh = _tempMesh;
                Selection.activeObject = currentMesh;
                SetVertexesData();
            }

            if (currentMesh == null || listData == null)
            {
                EndWindows();
                return;
            }

            DrawColorsMesh();

            DrawBottom();

            EndWindows();

            #region Local: Update(), FindPallete(), DrawTop(), DrawColorsMesh(),  DrawBottom()
            //=================================
            void SetVertexesData()
            {
                if (currentMesh != null)
                {
                    subMeshCount = currentMesh.subMeshCount;
                    FindPalette(NamePaletteFromMesh);
                    CreateColorsData();
                }
            }
            //=================================
            void FindPalette(string name)
            {
                foreach (var palette in Resources.FindObjectsOfTypeAll<PaletteVertexScriptable>())
                {
                    if (palette.name == name)
                    {
                        currentPalette = palette;
                        return;
                    }
                }
            }
            //=================================
            void DrawTop()
            {
                EditorGUILayout.Space(SPACE_WND);

                EditorGUILayout.LabelField(LABEL_PALETTE_PATH, PALETTE_PATH, EditorStyles.boldLabel);
                _tempPalette = EditorGUILayout.ObjectField(LABEL_PALETTE, currentPalette, typePalette, false) as PaletteVertexScriptable;
                _tempMesh = EditorGUILayout.ObjectField(LABEL_MESH, currentMesh, typeMesh, false) as Mesh;

                EditorGUILayout.Space();
            }
            //=================================
            void DrawColorsMesh()
            {
                _isEditName = EditorGUILayout.ToggleLeft(LABEL_EDIT_NAMES, _isEditName);
                if (GUILayout.Button(LABEL_RELOAD_BUTTON))
                    SetVertexesData();
                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical(GUI.skin.window);
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                foreach (var data in listData)
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

                if (subMeshCount > 1)
                {
                    Color prevColor = GUI.color;
                    GUI.color = isInvert ? Color.red : Color.green;
                    isInvert = EditorGUILayout.ToggleLeft(LABEL_IS_INVERT, isInvert);
                    GUI.color = prevColor;
                }

                EditorGUILayout.Space();
                if (GUILayout.Button(LABEL_MESH_BUTTON))
                    SaveMesh();

                EditorGUILayout.Space(SPACE_WND);
            }
            #endregion
        }

        private void CreateColorsData()
        {
            if (currentMesh == null)
                return;

            nameMesh = PREFIX_MESH.Concat(currentMesh.name);

            Color[] colors = currentMesh.colors;
            vertexCount = colors.Length;

            if (vertexCount <= 0)
            {
                listData = null;
                return;
            }

            isInvert = currentPalette != null && currentPalette.IsInvert && subMeshCount > 1;

            HashSet<int>[] subMeshes = new HashSet<int>[subMeshCount];
            listData = new ListVertexMaterials[subMeshCount];

            for (int i = 0; i < subMeshCount; i++)
            {
                listData[i] = CreateInstance<ListVertexMaterials>().Initialize(i);
                subMeshes[i] = new(currentMesh.GetTriangles(i));
            }

            Color currentColor;
            ListVertexMaterials listVert = null;
            colorsCount = 0;
            for (int vertex = 0; vertex < vertexCount; vertex++)
            {
                for (int j = 0; j < subMeshCount; j++)
                {
                    if (subMeshes[j].Contains(vertex))
                    {
                        listVert = listData[j];
                        break;
                    }
                }

                currentColor = colors[vertex];

                if (listVert.TryInsert(currentColor, vertex))
                    continue;

                if (currentPalette != null && currentPalette.Count > colorsCount)
                    listVert.Add(currentColor, currentPalette[colorsCount], vertex);
                else
                    listVert.Add(currentColor, vertex);

                colorsCount++;
            }
        }

        private void SetPalette()
        {
            if (currentMesh == null || currentPalette == null || listData == null)
                return;

            isInvert = currentPalette.IsInvert;

            int colorsCount = 0;
            ListVertexMaterials listVertexes;
            for (int i = 0; i < subMeshCount; i++)
            {
                listVertexes = listData[i];
                if (listVertexes == null)
                    return;

                for (int j = 0; j < listVertexes.Count; j++)
                {
                    if (currentPalette.Count <= colorsCount)
                        return;

                    listVertexes[j].Material = currentPalette[colorsCount];
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
            Color32[] colors = new Color32[vertexCount];
            Vector2[] uvs = new Vector2[vertexCount];

            foreach (var lisData in listData)
            {
                if (lisData.IsEdit)
                    lisData.CopyValues(colors, uvs);
                else
                    lisData.CopyValues(colors, uvs, currentMesh.colors32, currentMesh.uv);
            }

            newMesh.colors32 = colors;
            newMesh.uv = uvs;
            newMesh.Optimize();
            newMesh.UploadMeshData(true);

            AssetDatabase.CreateAsset(newMesh, path);
            AssetDatabase.SaveAssets();

            Debug.Log($"Mesh is saved: {path}");

            //EditorUtility.FocusProjectWindow();
            //Selection.activeObject = newMesh;

            #region Local: SetPath(), SavePalette(), CopyMesh()
            //=================================
            bool SetPath(out string path)
            {
                path = EditorUtility.SaveFilePanelInProject("Save mesh", nameMesh, MESH_EXP, "", _saveFolder);

                if (string.IsNullOrEmpty(path))
                    return false;

                nameMesh = Path.GetFileNameWithoutExtension(path);
                _saveFolder = Path.GetDirectoryName(path);

                return true;
            }
            void SavePalette()
            {
                currentPalette = CreateInstance<PaletteVertexScriptable>();
                currentPalette.Initialize(NamePaletteFromMesh, isInvert, _isEditName, colorsCount);

                foreach (var lisData in listData)
                    foreach (var data in lisData)
                        currentPalette.Add(data.Material);

                string path = PALETTE_PATH.Concat(currentPalette.name, ".", PALETTE_EXP);
                AssetDatabase.CreateAsset(currentPalette, path);

                Debug.Log($"Palette is saved: {path}");
            }
            //=================================
            Mesh CopyMesh()
            {
                Mesh mesh = new()
                {
                    name = nameMesh,
                    vertices = currentMesh.vertices,
                    normals = currentMesh.normals,
                    bounds = currentMesh.bounds,
                    tangents = currentMesh.tangents ?? null,
                    uv2 = currentMesh.uv2 ?? null,
                };

                int count = mesh.subMeshCount = subMeshCount, increment = isInvert ? -1 : 1;
                for (int i = 0, j = isInvert ? count - 1 : 0; i < count; i++, j += increment)
                    mesh.SetTriangles(currentMesh.GetTriangles(i), j);

                if (isInvert)
                    mesh.RecalculateBounds();

                return mesh;
            }
            #endregion
        }
    }
}
