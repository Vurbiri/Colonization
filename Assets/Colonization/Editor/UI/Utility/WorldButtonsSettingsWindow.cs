using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri;
using Vurbiri.UI;
using VurbiriEditor.UI;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class WorldButtonsSettingsWindow : EditorWindow
	{
		#region Consts
		private const string NAME = "World Buttons Settings", MENU = MENU_UI_PATH + NAME;
        #endregion

        [SerializeField] private List<AWorldHintButton> _prefabs;
        [SerializeField] private List<AWorldHintButton> _scene;

        [SerializeField] private ColorBlock _colorBlock;
        [SerializeField] private ScaleBlockFloat _scaleBlock;
        private ScaleBlock _scaleBlockVector;

        private SerializedObject _self;
        private SerializedProperty _propertyPrefabs, _propertyScenes, _scaleBlockDrawer;
        private ColorBlockDrawer _colorBlockDrawer;

        private Vector2 _scrollPosParams, _scrollPosButtons;
        private bool _isSave;

        [MenuItem(MENU, false, 59)]
		private static void ShowWindow()
		{
			GetWindow<WorldButtonsSettingsWindow>(true, NAME);
		}
		
		private void OnEnable()
		{
            _self = new(this);
            _propertyPrefabs = _self.FindProperty("_prefabs");
            _propertyScenes = _self.FindProperty("_scene");

            _colorBlockDrawer = new(_self.FindProperty("_colorBlock"));
            _scaleBlockDrawer = _self.FindProperty("_scaleBlock");

            _prefabs = EUtility.FindComponentsPrefabs<AWorldHintButton>();
            _scene = new(FindObjectsByType<AWorldHintButton>(FindObjectsInactive.Include, FindObjectsSortMode.None));

            for (int i = _prefabs.Count - 1; i >= 0; i--)
                if (PrefabUtility.IsPartOfVariantPrefab(_prefabs[i]) && !PrefabUtility.IsAddedComponentOverride(_prefabs[i]))
                    _prefabs.RemoveAt(i);

            for (int i = _scene.Count - 1; i >= 0; i--)
                if (PrefabUtility.IsPartOfAnyPrefab(_scene[i].gameObject))
                    _scene.RemoveAt(i);

            if (_prefabs != null || _prefabs.Count != 0)
            {
                AWorldHintButton button = _prefabs[0];
                _colorBlock = button.colors;
                _scaleBlock = _scaleBlockVector = button.Scales;
            }

            _self.Update();
        }
		
		private void OnGUI()
		{
            if (_scene == null)
                return;

            BeginWindows();
            EditorGUILayout.BeginVertical(STYLES.borderLight);
            
            DrawParams();
            DrawApply();
            DrawButtons();

            EditorGUILayout.EndVertical();
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
                EditorGUILayout.BeginVertical(GUI.skin.window);
                _scrollPosParams = EditorGUILayout.BeginScrollView(_scrollPosParams);

                _colorBlockDrawer.DrawGUILayout();
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_scaleBlockDrawer);

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }
            //=================================
            void DrawButtons()
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(GUI.skin.window);
                _scrollPosButtons = EditorGUILayout.BeginScrollView(_scrollPosButtons);

                EditorGUILayout.PropertyField(_propertyPrefabs);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_propertyScenes);
                
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }
            //=================================
            void DrawApply()
            {
                if (GUILayout.Button("Apply"))
                {
                    _self.ApplyModifiedProperties();

                    _scaleBlockVector = _scaleBlock;
                    _scaleBlockVector.fadeDuration = _colorBlock.fadeDuration;

                    foreach (var button in _prefabs)
                        Apply(button, "Prefab");

                    foreach (var button in _scene)
                        Apply(button, "Scene object");

                    if(!_isSave)
                        Debug.Log("<b>No buttons with other parameters.</b>");
                }

                EditorGUILayout.Space();

                #region Local: Apply(..)
                //=================================
                void Apply(AWorldHintButton button, string type)
                {
                    bool isSave = false;
                    
                    if (isSave |= button.colors != _colorBlock)
                        button.colors = _colorBlock;
                    if (isSave |= button.Scales != _scaleBlockVector)
                        button.Scales = _scaleBlockVector;

                    var navigation = button.navigation;

                    if (isSave |= navigation.mode != Navigation.Mode.None)
                    {
                        navigation.mode = Navigation.Mode.None;
                        button.navigation = navigation;
                    }

                    if (isSave)
                    {
                        EditorUtility.SetDirty(button);
                        Debug.Log($"Applied to {type} <b>{button.gameObject.name}</b>");
                        _isSave = true;
                    }
                }
                #endregion
            }
            #endregion
        }

        private void OnDisable()
		{
            _scene = null;
            _prefabs = null;
            _self.Update();

        }


        [Serializable]
        private struct ScaleBlockFloat
        {
            [Range(0.75f, 1.25f)] public float normal;
            [Range(0.75f, 1.25f)] public float highlighted;
            [Range(0.75f, 1.25f)] public float pressed;
            [Range(0.75f, 1.25f)] public float selected;
            [Range(0.75f, 1.25f)] public float disabled;
            private float _fadeDuration;

            public static implicit operator ScaleBlock(ScaleBlockFloat value)
            {
                ScaleBlock scaleBlock = new()
                {
                    normal = new(value.normal, value.normal, value.normal),
                    highlighted = new(value.highlighted, value.highlighted, value.highlighted),
                    pressed = new(value.pressed, value.pressed, value.pressed),
                    selected = new(value.selected, value.selected, value.selected),
                    disabled = new(value.disabled, value.disabled, value.disabled),
                    fadeDuration = value._fadeDuration
                };
                return scaleBlock;
            }
            public static implicit operator ScaleBlockFloat(ScaleBlock value)
            {
                ScaleBlockFloat scaleBlock = new()
                {
                    normal = value.normal.x, 
                    highlighted = value.highlighted.x, 
                    pressed = value.pressed.x,
                    selected = value.selected.x,
                    disabled = value.disabled.x,
                    _fadeDuration = value.fadeDuration
                }; 
                return scaleBlock;
            }
        }

    }
}