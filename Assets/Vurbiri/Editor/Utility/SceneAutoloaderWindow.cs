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
        private const string SCENE_TYPE = "t:Scene";
        private const string LABEL_SCENE = "Start scene", LABEL_SAVE = "Save scene", LABEL_PATH = "Path";
        private const string KEY_SCENE = "MSA_StartScene", KEY_SAVE = "MSA_SaveScene", KEY_PATH = "MSA_Path";

        private static SceneAsset[] sceneAssets;
        private static string[] nameScenes;
        private static int[] idScenes;
        private static string[] guids;
        private static readonly string[] arrPaths = { "Assets" };

        private static int startScene = 0, countScenes = 0;
        private static bool saveScene = true;

        private string _path;
        private int _startSceneTemp;

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<SceneAutoloaderWindow>(true, NAME);
        }

        private void OnGUI()
        {
            if (Application.isPlaying)
                return;

            BeginWindows();
            EditorGUILayout.Space(12);
            _path = EditorGUILayout.DelayedTextField(LABEL_PATH, arrPaths[0]);
            _startSceneTemp = EditorGUILayout.IntPopup(LABEL_SCENE, startScene, nameScenes, idScenes);
            EditorGUILayout.Space(12);
            saveScene = EditorGUILayout.Toggle(LABEL_SAVE, saveScene);
            EndWindows();

            if (_startSceneTemp != startScene || _path != arrPaths[0])
            {
                arrPaths[0] = _path;
                startScene = _startSceneTemp;
                UpdateListOfScenes();
            }
        }

        private void OnDisable()
        {
            Save();
        }

        private static void UpdateListOfScenes()
        {
            if (!AssetDatabase.IsValidFolder(arrPaths[0]))
                return;

            guids = AssetDatabase.FindAssets(SCENE_TYPE, arrPaths);

            countScenes = guids.Length;

            sceneAssets = new SceneAsset[countScenes];
            nameScenes = new string[countScenes];
            idScenes = new int[countScenes];

            if (countScenes == 0)
                return;

            for (int i = 0; i < countScenes; i++)
            {
                sceneAssets[i] = AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(guids[i]));
                nameScenes[i] = sceneAssets[i].name;
                idScenes[i] = i;
            }

            if (startScene < 0 || startScene >= countScenes)
                startScene = 0;

            EditorSceneManager.playModeStartScene = sceneAssets[startScene];
        }

        private static void OnModeStateChanged(PlayModeStateChange change)
        {
            if (saveScene && !EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorApplication.ExitPlaymode();
        }

        private static void Load()
        {
            if (EditorPrefs.HasKey(KEY_SCENE))
                startScene = EditorPrefs.GetInt(KEY_SCENE);

            if (EditorPrefs.HasKey(KEY_SAVE))
                saveScene = EditorPrefs.GetBool(KEY_SAVE);

            if (EditorPrefs.HasKey(KEY_PATH))
                arrPaths[0] = EditorPrefs.GetString(KEY_PATH);
        }

        private static void Save()
        {
            EditorPrefs.SetInt(KEY_SCENE, startScene);
            EditorPrefs.SetBool(KEY_SAVE, saveScene);
            EditorPrefs.SetString(KEY_PATH, arrPaths[0]);
        }

        #region Nested: InitializeOnLoad, Postprocessor
        //*******************************************************
        [InitializeOnLoad]
        private static class InitializeOnLoad
        {
            static InitializeOnLoad()
            {
                Debug.Log("SceneAutoloader - InitializeOnLoad");

                Load();

                EditorApplication.playModeStateChanged -= OnModeStateChanged;
                EditorApplication.playModeStateChanged += OnModeStateChanged;
            }
        }
        //*******************************************************
        private class Postprocessor : AssetPostprocessor
        {
            private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
            {
                UpdateListOfScenes();
            }
        }
        #endregion
    }
}





