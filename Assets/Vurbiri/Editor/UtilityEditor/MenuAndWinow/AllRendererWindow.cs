using System;
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

        private readonly Type[] _types = {typeof(MeshRenderer), typeof(SkinnedMeshRenderer), typeof(SpriteRenderer), typeof(ParticleSystemRenderer) };
        private readonly string[] _names;

        [SerializeField] private List<Renderer> _renderersPrefabs;
        [SerializeField] private List<Renderer> _renderersScene;

        private MotionVectorGenerationMode _motionVector = MotionVectorGenerationMode.Object;
        private LightProbeUsage _lightProbe;
        private ReflectionProbeUsage _reflectionProbe;
        private bool _occlusion;

        private SerializedObject _self;
        private SerializedProperty _propertyPrefabs, _propertyScenes;
        private Vector2 _scrollPos;
        private bool _isSave;
        private int[] _typeId = new int[2];

        public AllRendererWindow()
        {
            int count = _types.Length;
            _names = new string[count];
            for (int i = 0; i < count; i++)
                _names[i] = _types[i].Name;
        }

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
            _lightProbe = renderer.lightProbeUsage; 
            _reflectionProbe = renderer.reflectionProbeUsage;
            _occlusion = renderer.allowOcclusionWhenDynamic;
        }

        private void OnGUI()
        {
            if (_renderersPrefabs == null || _renderersScene == null)
                return;

            BeginWindows();
            DrawParams();
            DrawApply();
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
                _lightProbe = (LightProbeUsage)EditorGUILayout.EnumPopup("Light Probes", _lightProbe);
                _reflectionProbe = (ReflectionProbeUsage)EditorGUILayout.EnumPopup("Reflection Probes", _reflectionProbe);
                _occlusion = EditorGUILayout.Toggle("Dynamic Occlusion", _occlusion);
                EditorGUILayout.Space();
            }
            //=================================
            void DrawRenderer()
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(GUI.skin.window);
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                DrawSelect(_renderersScene, 0);
                EditorGUILayout.PropertyField(_propertyScenes);
                EditorGUILayout.Space();
                DrawSelect(_renderersPrefabs, 1);
                EditorGUILayout.PropertyField(_propertyPrefabs);

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }
            //=================================
            void DrawSelect(List<Renderer> renderers, int indexTypeId)
            {
                bool onClick;
                Rect rect = EditorGUILayout.BeginVertical();
                {
                    rect.height = EditorGUIUtility.singleLineHeight;
                    Rect position = rect;

                    position.width = rect.width * 0.5f - 1f;
                    onClick = GUI.Button(position, "Select");

                    position.x = rect.width * 0.5f + 1f;
                    _typeId[indexTypeId] = EditorGUI.Popup(position, _typeId[indexTypeId], _names);

                    EditorGUILayout.Space(rect.height);
                }
                EditorGUILayout.EndVertical();
                if (onClick)
                {
                    List<GameObject> gameObjects = new(renderers.Count);

                    Type type = _types[_typeId[indexTypeId]];
                    foreach (var renderer in renderers)
                        if (renderer.GetType() == type)
                            gameObjects.Add(renderer.gameObject);

                    Selection.objects = gameObjects.ToArray();
                    Debug.Log($"Selected <b>{gameObjects.Count}</b> object(s)");
                }
            }
            //=================================
            void DrawApply()
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
                    if (isSave |= renderer.lightProbeUsage != _lightProbe)
                        renderer.lightProbeUsage = _lightProbe;
                    if (isSave |= renderer.reflectionProbeUsage != _reflectionProbe)
                        renderer.reflectionProbeUsage = _reflectionProbe;
                    if (isSave |= renderer.allowOcclusionWhenDynamic != _occlusion)
                        renderer.allowOcclusionWhenDynamic = _occlusion;

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
