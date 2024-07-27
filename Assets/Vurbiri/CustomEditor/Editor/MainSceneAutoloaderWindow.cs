using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Vurbiri
{
    public class MainSceneAutoloaderWindow : EditorWindow
    {
        private const string NAME = "Scene Autoloader", MENU = "Window/" + NAME;
        private const string LABEL_SCAENE = "Start scene", LABEL_SAVE = "Save scene", LABEL_BUTTON = "Update list of scenes";
        private const string KEY_SCAENE = "MSA_StartScene", KEY_SAVE = "MSA_SaveScene";

        private int _countScenes;
        private string[] _nameScenes;
        private int[] _idScenes;

        private static int startScene = 0;
        private static bool saveScene = true;

        private void OnEnable()
        {
            UpdateScenes();

            if (EditorPrefs.HasKey(KEY_SCAENE))
                startScene = EditorPrefs.GetInt(KEY_SCAENE);

            if (EditorPrefs.HasKey(KEY_SAVE))
                saveScene = EditorPrefs.GetBool(KEY_SAVE);
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(12);
            startScene = EditorGUILayout.IntPopup(LABEL_SCAENE, startScene, _nameScenes, _idScenes);
            EditorGUILayout.Space(12);
            saveScene = EditorGUILayout.Toggle(LABEL_SAVE, saveScene);
            EditorGUILayout.Space(12);
            if (GUILayout.Button(LABEL_BUTTON))
                UpdateScenes();
        }

        private void OnDisable()
        {
            EditorPrefs.SetInt(KEY_SCAENE, startScene);
            EditorPrefs.SetBool(KEY_SAVE, saveScene);
        }

        private void UpdateScenes()
        {
            _countScenes = EditorBuildSettings.scenes.Length;
            _nameScenes = new string[_countScenes];
            _idScenes = new int[_countScenes];

            for (int i = 0; i < _countScenes; i++)
            {
                _nameScenes[i] = $"{Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path)} ({i})";
                _idScenes[i] = i;
            }
        }

        [MenuItem(MENU)]
        public static void ShowWindow()
        {
            var wnd = GetWindow<MainSceneAutoloaderWindow>();
            wnd.titleContent = new GUIContent(NAME);
        }

        #region Nested: MainSceneAutoloader
        //*******************************************************
        [InitializeOnLoad]
        private static class MainSceneAutoloader
        {
            static MainSceneAutoloader()
            {
                EditorApplication.playModeStateChanged += OnModeStateChanged;

                int startScene = MainSceneAutoloaderWindow.startScene;
                if (EditorBuildSettings.scenes.Length <= startScene)
                    return;

                EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[startScene].path);
            }

            private static void OnModeStateChanged(PlayModeStateChange change)
            {
                if (MainSceneAutoloaderWindow.saveScene && !EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
                    if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        EditorApplication.ExitPlaymode();
            }
        }
        #endregion
    }
}





