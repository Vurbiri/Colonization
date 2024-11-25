//Assets\Vurbiri\Editor\Utility\SceneAutoloaderWindow.cs
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static VurbiriEditor.CONST_EDITOR;

namespace VurbiriEditor
{
    internal class SceneAutoloaderWindow : EditorWindow
    {
        private const string NAME = "Scene Autoloader", MENU = MENU_PATH + NAME;
        private const string SCENE_TYPE = "t:Scene", ASSETS = "Assets";
        private const string LABEL_SCENE = "Start scene", LABEL_SAVE = "Save Assets and Scene", LABEL_PATH = "Folder", LABEL_BUTTON = "Set Folder";
        private const string KEY_SCENE = "MSA_StartScene", KEY_SAVE = "MSA_SaveScene", KEY_PATH = "MSA_Path";

        private static readonly string[] arrPaths = { ASSETS };
        private static int startScene = 0;
        private static bool isSaveScene = true;

        private string[] _nameScenes;
        private int[] _idScenes;
        private string _path;
        private int _startSceneTemp;

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<SceneAutoloaderWindow>(true, NAME);
        }

        private void OnEnable()
        {
            if(!UpdateListOfScenes())
            {
                _nameScenes = new string[0];
                _idScenes = new int[0];
            }
        }

        private void OnGUI()
        {
            if (Application.isPlaying)
                return;

            BeginWindows();
            DrawButton();
            _startSceneTemp = EditorGUILayout.IntPopup(LABEL_SCENE, startScene, _nameScenes, _idScenes);
            EditorGUILayout.Space();
            isSaveScene = EditorGUILayout.Toggle(LABEL_SAVE, isSaveScene);
            EndWindows();

            if (_startSceneTemp != startScene || _path != arrPaths[0])
            {
                arrPaths[0] = _path;
                startScene = _startSceneTemp;
                UpdateListOfScenes();
            }

            #region Local: DrawButton()
            //=================================
            void DrawButton()
            {
                EditorGUILayout.Space();
                if (GUILayout.Button(LABEL_BUTTON))
                {
                    _path = EditorUtility.OpenFolderPanel(LABEL_PATH, arrPaths[0], "");

                    int index = _path.IndexOf(ASSETS);
                    if (index > 0) _path = _path[index..];
                }

                if (string.IsNullOrEmpty(_path) || !AssetDatabase.IsValidFolder(_path))
                    _path = arrPaths[0];

                EditorGUILayout.LabelField(LABEL_PATH, _path);
                EditorGUILayout.Space();
            }
            //=================================
            #endregion
        }

        private void OnDisable()
        {
            Save();
        }

        private bool UpdateListOfScenes()
        {
            if (!AssetDatabase.IsValidFolder(arrPaths[0]))
                return false;

            string[] guids = AssetDatabase.FindAssets(SCENE_TYPE, arrPaths);

            int countScenes = guids.Length;
            if (countScenes == 0)
            {
                startScene = 0;
                return false;
            }

            startScene = Mathf.Clamp(startScene, 0, countScenes - 1);
            var sceneAssets = new SceneAsset[countScenes];
            _nameScenes = new string[countScenes];
            _idScenes = new int[countScenes];

            for (int i = 0; i < countScenes; i++)
            {
                sceneAssets[i] = AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(guids[i]));
                _nameScenes[i] = sceneAssets[i].name;
                _idScenes[i] = i;
            }

            EditorSceneManager.playModeStartScene = sceneAssets[startScene];

            return true;
        }

        private static void OnModeStateChanged(PlayModeStateChange change)
        {
            if (isSaveScene & change == PlayModeStateChange.ExitingEditMode)
            {
                AssetDatabase.SaveAssets();
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorApplication.ExitPlaymode();
            }
        }

        private static void Load()
        {
            if (EditorPrefs.HasKey(KEY_SCENE))
                startScene = EditorPrefs.GetInt(KEY_SCENE);

            if (EditorPrefs.HasKey(KEY_SAVE))
                isSaveScene = EditorPrefs.GetBool(KEY_SAVE);

            if (EditorPrefs.HasKey(KEY_PATH))
                arrPaths[0] = EditorPrefs.GetString(KEY_PATH);
        }

        private void Save()
        {
            EditorPrefs.SetInt(KEY_SCENE, startScene);
            EditorPrefs.SetBool(KEY_SAVE, isSaveScene);
            EditorPrefs.SetString(KEY_PATH, arrPaths[0]);
        }

        #region Nested: InitializeOnLoad, Postprocessor
        //*******************************************************
        [InitializeOnLoad]
        private static class InitializeOnLoad
        {
            static InitializeOnLoad()
            {
                Debug.Log("SceneAutoloader [InitializeOnLoad]");

                Load();

                EditorApplication.playModeStateChanged -= OnModeStateChanged;
                EditorApplication.playModeStateChanged += OnModeStateChanged;
            }
        }
        //*******************************************************
        #endregion
    }
}





