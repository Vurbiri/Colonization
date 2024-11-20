using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static VurbiriEditor.CONST_EDITOR;

namespace VurbiriEditor
{
    public class AllRenderer : EditorWindow
    {
        private const string NAME = "All Renderer Settings", MENU = MENU_PATH + NAME;

        private Vector2 _scrollPos;
        private List<Renderer> _renderersPrefabs;
        private Renderer[] _renderersScene;
        private MotionVectorGenerationMode _motionVector = MotionVectorGenerationMode.Object;
        private LightProbeUsage _probeUsage;


        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<AllRenderer>(true, NAME);
        }

        private void OnEnable()
        {
            _renderersScene = FindObjectsByType<Renderer>(FindObjectsSortMode.None);
            _renderersPrefabs = Utility.FindAllComponentsPrefabs<Renderer>();
            if (_renderersPrefabs == null || _renderersPrefabs.Count == 0)
                return;

            Renderer renderer = _renderersPrefabs[^1];
            _motionVector = renderer.motionVectorGenerationMode;
            _probeUsage = renderer.lightProbeUsage;
        }

        private void OnGUI()
        {
            if (_renderersPrefabs == null || _renderersPrefabs.Count == 0 )
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
                Type type = typeof(Renderer);
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(GUI.skin.window);
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                EditorGUILayout.LabelField("SCENE:");
                foreach (Renderer renderer in _renderersScene)
                    EditorGUILayout.ObjectField(renderer, type, true);
                
                EditorGUILayout.Space(12);
                EditorGUILayout.LabelField("PREFABS:");
                foreach (Renderer renderer in _renderersPrefabs)
                    EditorGUILayout.ObjectField(renderer, type, true);
                EditorGUILayout.Space();

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }
            //=================================
            void DrawBottom()
            {
                if (GUILayout.Button("Apply"))
                {
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

                    AssetDatabase.SaveAssets();
                }

                EditorGUILayout.Space();
            }
            #endregion
        }
    }
}
