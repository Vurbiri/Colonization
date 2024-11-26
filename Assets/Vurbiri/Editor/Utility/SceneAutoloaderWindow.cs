//Assets\Vurbiri\Editor\Utility\SceneAutoloaderWindow.cs
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static VurbiriEditor.CONST_EDITOR;
using static VurbiriEditor.InitializeOnLoad;

namespace VurbiriEditor
{
    internal class SceneAutoloaderWindow : EditorWindow
    {
        private const string NAME = "Scene Autoloader", MENU = MENU_PATH + NAME;
        private const string LABEL_SCENE = "Start scene", LABEL_SAVE = "Save Assets and Scene", LABEL_PATH = "Folder", LABEL_BUTTON = "Set Folder";

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
            UpdateListScenes();
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
                UpdateListScenes();
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

        private void UpdateListScenes()
        {
            int countScenes = sceneAssets.Length;

            _nameScenes = new string[countScenes];
            _idScenes = new int[countScenes];

            for (int i = 0; i < countScenes; i++)
            {
                _nameScenes[i] = sceneAssets[i].name;
                _idScenes[i] = i;
            }
        }

        private void Save()
        {
            EditorPrefs.SetInt(KEY_SCENE, startScene);
            EditorPrefs.SetBool(KEY_SAVE, isSaveScene);
            EditorPrefs.SetString(KEY_PATH, arrPaths[0]);
        }
    }

    internal class InitializeOnLoad
    {
        public const string KEY_SCENE = "MSA_StartScene", KEY_SAVE = "MSA_SaveScene", KEY_PATH = "MSA_Path";
        public const string SCENE_TYPE = "t:Scene";

        public static SceneAsset[] sceneAssets;

        public static readonly string[] arrPaths = { ASSETS };
        public static int startScene = 0;
        public static bool isSaveScene = true;

        [InitializeOnLoadMethod]
        static void OnProjectLoadedInEditor()
        {
            Debug.Log("[SceneAutoloader]");

            Load(); SetupScenes();

            EditorApplication.playModeStateChanged -= OnModeStateChanged;
            EditorApplication.playModeStateChanged += OnModeStateChanged;
        }

        public static void OnModeStateChanged(PlayModeStateChange change)
        {
            if (isSaveScene & change == PlayModeStateChange.ExitingEditMode)
            {
                AssetDatabase.SaveAssets();
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorApplication.ExitPlaymode();
            }
        }

        public static void Load()
        {
            if (EditorPrefs.HasKey(KEY_SCENE))
                startScene = EditorPrefs.GetInt(KEY_SCENE);

            if (EditorPrefs.HasKey(KEY_SAVE))
                isSaveScene = EditorPrefs.GetBool(KEY_SAVE);

            if (EditorPrefs.HasKey(KEY_PATH))
                arrPaths[0] = EditorPrefs.GetString(KEY_PATH);
        }

        public static void SetupScenes()
        {
            if (!AssetDatabase.IsValidFolder(arrPaths[0]))
            {
                sceneAssets = new SceneAsset[0];
                return;
            }

            string[] guids = AssetDatabase.FindAssets(SCENE_TYPE, arrPaths);
            int countScenes = guids.Length;

            sceneAssets = new SceneAsset[countScenes];
            for (int i = 0; i < countScenes; i++)
                sceneAssets[i] = AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(guids[i]));

            if (startScene < 0 | startScene >= countScenes) startScene = 0;
            EditorSceneManager.playModeStartScene = sceneAssets[startScene];
        }

    }
}





