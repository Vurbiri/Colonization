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

        [SerializeField] private List<AWorldHintButton> _buttonsPrefabs;
        [SerializeField] private List<AWorldHintButton> _buttonsScene;

        [SerializeField] private ColorBlock _colorBlock;
        [SerializeField] private ScaleBlock _scaleBlock;

        private SerializedObject _self;
        private SerializedProperty _propertyPrefabs, _propertyScenes;
        private ColorBlockDrawer _colorBlockDrawer;
        private ScaleBlockDrawer _scaleBlockDrawer;

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
            _propertyPrefabs = _self.FindProperty("_buttonsPrefabs");
            _propertyScenes = _self.FindProperty("_buttonsScene");
            _colorBlockDrawer = new(_self.FindProperty("_colorBlock"));
            _scaleBlockDrawer = new(_self.FindProperty("_scaleBlock"));

            _buttonsPrefabs = EUtility.FindComponentsPrefabs<AWorldHintButton>();
            _buttonsScene = new(FindObjectsByType<AWorldHintButton>(FindObjectsInactive.Include, FindObjectsSortMode.None));

            for (int i = _buttonsPrefabs.Count - 1; i >= 0; i--)
                if (PrefabUtility.IsPartOfVariantPrefab(_buttonsPrefabs[i]) && !PrefabUtility.IsAddedComponentOverride(_buttonsPrefabs[i]))
                    _buttonsPrefabs.RemoveAt(i);

            for (int i = _buttonsScene.Count - 1; i >= 0; i--)
                if (PrefabUtility.IsPartOfAnyPrefab(_buttonsScene[i].gameObject))
                    _buttonsScene.RemoveAt(i);

            if (_buttonsPrefabs != null || _buttonsPrefabs.Count != 0)
            {
                AWorldHintButton button = _buttonsPrefabs[0];
                _colorBlock = button.colors;
                _scaleBlock = button.Scales;
            }

            _self.Update();
        }
		
		private void OnGUI()
		{
            if (_buttonsScene == null)
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
                _scaleBlockDrawer.DrawGUILayout();

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

                    foreach (var button in _buttonsPrefabs)
                        Apply(button, "Prefab");

                    foreach (var button in _buttonsScene)
                        Apply(button, "Scene object");
                }

                EditorGUILayout.Space();

                #region Local: Apply(..)
                //=================================
                void Apply(AWorldHintButton button, string type)
                {
                    bool isSave = false;
                    
                    if (isSave |= button.colors != _colorBlock)
                        button.colors = _colorBlock;
                    if (isSave |= button.Scales != _scaleBlock)
                        button.Scales = _scaleBlock;

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
            _buttonsScene = null;
            _buttonsPrefabs = null;
            _self.Update();

        }
	}
}