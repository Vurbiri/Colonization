//Assets\Vurbiri\Editor\UtilityEditor\SceneAutoloaderWindow.cs
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static VurbiriEditor.CONST_EDITOR;

namespace VurbiriEditor
{
    internal class SceneAutoloaderWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Scene Autoloader", MENU = MENU_PATH + NAME;
        private const string LABEL_SCENE = "Start scene", LABEL_SAVE = "Save assets when playing";
        private const string KEY_SAVE = "MSA_SaveScene", KEY_PATH = "MSA_PathScene";
        public const string SCENE_TYPE = "t:Scene";
        #endregion

        private static bool s_isSaveScene = true;

        private readonly System.Type _typeSceneAsset = typeof(SceneAsset);
        private SceneAsset _sceneAsset;

        [MenuItem(MENU, false, 44)]
        private static void ShowWindow()
        {
            GetWindow<SceneAutoloaderWindow>(true, NAME);
        }

        private void OnEnable()
        {
            _sceneAsset = EditorSceneManager.playModeStartScene;
        }

        private void OnGUI()
        {
            if (Application.isPlaying)
                return;

            BeginWindows();
            {
                EditorGUILayout.Space(10f);
                EditorGUILayout.LabelField(NAME, STYLES.H1);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.Space();
                    _sceneAsset = (SceneAsset)EditorGUILayout.ObjectField(LABEL_SCENE, _sceneAsset, _typeSceneAsset, false);
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.Space();
                        s_isSaveScene = EditorGUILayout.ToggleLeft(LABEL_SAVE, s_isSaveScene);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndVertical();
            }
            EndWindows();

            if (_sceneAsset != EditorSceneManager.playModeStartScene)
            {
                EditorPrefs.SetString(KEY_PATH, _sceneAsset != null ? AssetDatabase.GetAssetPath(_sceneAsset) : string.Empty);
                EditorSceneManager.playModeStartScene = _sceneAsset;
            }
        }

        private void OnDisable()
        {
            EditorPrefs.SetBool(KEY_SAVE, s_isSaveScene);
        }

        [InitializeOnLoadMethod]
        private static void OnProjectLoadedInEditor()
        {
            Load();

            EditorApplication.playModeStateChanged -= OnModeStateChanged;
            EditorApplication.playModeStateChanged += OnModeStateChanged;
        }

        private static void OnModeStateChanged(PlayModeStateChange change)
        {
            if (s_isSaveScene & change == PlayModeStateChange.ExitingEditMode)
            {
                AssetDatabase.SaveAssets();
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorApplication.ExitPlaymode();
            }
        }

        private static void Load()
        {
            if (EditorSceneManager.playModeStartScene != null)
            {
                Debug.Log($"<color=yellow>[SceneAutoloader] Start scene: <b>{EditorSceneManager.playModeStartScene.name}</b></color>");
                return;
            }

            if (EditorPrefs.HasKey(KEY_SAVE))
                s_isSaveScene = EditorPrefs.GetBool(KEY_SAVE);

            string path = null;
            if (EditorPrefs.HasKey(KEY_PATH))
                path = EditorPrefs.GetString(KEY_PATH);

            if (string.IsNullOrEmpty(path))
            {
                Debug.Log("<color=yellow>[SceneAutoloader] Start scene: <b>current</b></color>");
                return;
            }

            SceneAsset startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            if (startScene != null)
            {
                EditorSceneManager.playModeStartScene = startScene;
                Debug.Log($"<color=yellow>[SceneAutoloader] Set start scene: <b>{startScene.name}</b></color>");
            }
            else
            {
                Debug.LogWarning($"[SceneAutoloader] Could not find scene: {path}");
            }
        }
    }

}





