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
                
        [SerializeField] private List<Renderer> _renderersPrefabs;
        [SerializeField] private List<Renderer> _renderersScene;

        private MotionVectorGenerationMode _motionVector = MotionVectorGenerationMode.Object;
        private LightProbeUsage _probeUsage;
        private SerializedObject _self;
        private SerializedProperty _propertyPrefabs, _propertyScenes;
        private Vector2 _scrollPos;
        private bool _isSave;

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

            _renderersScene = new(FindObjectsByType<Renderer>(FindObjectsInactive.Include ,FindObjectsSortMode.None));
            _renderersPrefabs = EUtility.FindComponentsPrefabs<Renderer>();

            for (int i = _renderersScene.Count - 1; i >= 0; i--)
                if (PrefabUtility.IsPartOfAnyPrefab(_renderersScene[i].gameObject))
                    _renderersScene.RemoveAt(i);

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
            DrawButton();
            DrawRenderer();
            EndWindows();

            if (_isSave)
            {
                AssetDatabase.SaveAssets();
                EditorSceneManager.SaveOpenScenes();
                _isSave = false;
            }

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
            void DrawButton()
            {
                if (GUILayout.Button("Apply"))
                {
                    _self.ApplyModifiedProperties();

                    _renderersPrefabs.ForEach(Apply);
                    _renderersScene.ForEach(Apply);
                }

                EditorGUILayout.Space();

                #region Local: Apply(..)
                //=================================
                void Apply(Renderer renderer)
                {
                    bool isSave = false;
                    if (isSave |= renderer.motionVectorGenerationMode != _motionVector)
                        renderer.motionVectorGenerationMode = _motionVector;
                    if (isSave |= renderer.lightProbeUsage != _probeUsage)
                        renderer.lightProbeUsage = _probeUsage;

                    if (isSave)
                    {
                        EditorUtility.SetDirty(renderer);
                        Debug.Log($"Applied to <b>{renderer.gameObject.name}</b>");
                        _isSave = true;
                    }
                }
                #endregion
            }
            #endregion
        }

        private void OnDisable()
        {
            _renderersScene = null;
            _renderersPrefabs = null;
            _self.Update();

            AssetDatabase.SaveAssets();
        }
    }
}
