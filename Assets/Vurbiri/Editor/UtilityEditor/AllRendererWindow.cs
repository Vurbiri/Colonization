using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using Vurbiri;
using static VurbiriEditor.CONST_EDITOR;

namespace VurbiriEditor
{
    internal class AllRendererWindow : EditorWindow
    {
        private const string NAME = "All Renderer Settings", MENU = MENU_PATH + NAME;

        private Vector2 _scrollPos;
        [SerializeField] private List<Renderer> _renderersPrefabs;
        [SerializeField] private Renderer[] _renderersScene;
        private MotionVectorGenerationMode _motionVector = MotionVectorGenerationMode.Object;
        private LightProbeUsage _probeUsage;
        SerializedObject _self;
        SerializedProperty _propertyPrefabs, _propertyScenes;

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<AllRendererWindow>(true, NAME);
        }

        private void OnEnable()
        {
            _self = new(this);
            _propertyScenes = _self.FindProperty("_renderersScene");
            _propertyPrefabs = _self.FindProperty("_renderersPrefabs");

            _renderersScene = FindObjectsByType<Renderer>(FindObjectsInactive.Include ,FindObjectsSortMode.None);
            _renderersPrefabs = EUtility.FindComponentsPrefabs<Renderer>();

            _self.Update();

            if (_renderersPrefabs == null || _renderersPrefabs.Count == 0)
                return;

            Renderer renderer = _renderersPrefabs[^1];
            _motionVector = renderer.motionVectorGenerationMode;
            _probeUsage = renderer.lightProbeUsage;
        }

        private void OnGUI()
        {
            if (_renderersPrefabs == null || _renderersScene == null)
                return;

            BeginWindows();
            DrawParams();
            DrawBottom();
            DrawRenderer();
            EndWindows();

            #region Local: DrawParams(), DrawRenderer(), DrawBottom()
            //=================================
            void DrawParams()
            {
                EditorGUILayout.Space(12);
                _motionVector = (MotionVectorGenerationMode)EditorGUILayout.EnumPopup("Motion Vectors", _motionVector);
                _probeUsage = (LightProbeUsage)EditorGUILayout.EnumPopup("Light Probes", _probeUsage);
                EditorGUILayout.Space();
            }
            //=================================
            void DrawRenderer()
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(GUI.skin.window);
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                EditorGUILayout.PropertyField(_propertyScenes);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_propertyPrefabs);

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }
            //=================================
            void DrawBottom()
            {
                if (GUILayout.Button("Apply"))
                {
                    _self.ApplyModifiedProperties();

                    foreach (Renderer renderer in _renderersPrefabs)
                    {
                        renderer.motionVectorGenerationMode = _motionVector;
                        renderer.lightProbeUsage = _probeUsage;
                    }
                    foreach (Renderer renderer in _renderersScene)
                    {
                        renderer.motionVectorGenerationMode = _motionVector;
                        renderer.lightProbeUsage = _probeUsage;
                    }

                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    AssetDatabase.SaveAssets();
                }

                EditorGUILayout.Space();
            }
            #endregion
        }

        private void OnDisable()
        {
            _renderersScene = null;
            _renderersPrefabs = null;
            _self.Update();
        }
    }
}
