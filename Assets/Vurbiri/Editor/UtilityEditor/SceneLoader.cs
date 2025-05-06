//Assets\Vurbiri\Editor\UtilityEditor\SceneLoader.cs
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Vurbiri;
using static VurbiriEditor.CONST_EDITOR;
using UScene = UnityEngine.SceneManagement.Scene;

namespace VurbiriEditor
{
    public class SceneLoader : EditorWindow
	{
		#region Consts
		private const string NAME = "Scene Loader", MENU = MENU_PATH + NAME;
        #endregion

        private static SceneField sceneField;

        [MenuItem(MENU, false, 77)]
		private static void ShowWindow()
		{
			GetWindow<SceneLoader>(NAME);
		}
		
		private void OnEnable()
		{
            sceneField = new(OpenScene);

            EditorSceneManager.activeSceneChangedInEditMode += ChangedActiveScene;
            SceneManager.activeSceneChanged += ChangedActiveScene;
        }

        public void CreateGUI()
        {
            rootVisualElement.Add(sceneField);
        }

        public void Update()
        {
            sceneField.SetEnabled(!Application.isPlaying);
        }

        private void OnDisable()
        {
            sceneField = sceneField.Dispose(OpenScene);

            EditorSceneManager.activeSceneChangedInEditMode -= ChangedActiveScene;
            SceneManager.activeSceneChanged -= ChangedActiveScene;
        }

        private static void OpenScene(ChangeEvent<Scene> evt)
        {
            if (EditorSceneManager.SaveOpenScenes())
                EditorSceneManager.OpenScene(evt.newValue);
        }

        private static void ChangedActiveScene(UScene current, UScene next)
        {
            sceneField.SetValueWithoutNotify(next);
        }

        #region Nested: SceneModificationProcessor, SceneField, 
        // ================== SceneModificationProcessor ==========================
        public class SceneModificationProcessor : AssetModificationProcessor
        {
            private const string SCENE_EXT = ".unity";

            public static void OnWillCreateAsset(string assetPath)
            {
                if (IsExecute(assetPath))
                    sceneField.AddItem(assetPath);
            }

            public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
            {
                if (IsExecute(assetPath))
                    sceneField.RemoveItem(assetPath);

                return AssetDeleteResult.DidNotDelete;
            }

            public static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
            {
                if (IsExecute(sourcePath))
                    sceneField.ReplaceItem(sourcePath, destinationPath);

                return AssetMoveResult.DidNotMove;
            }

            private static bool IsExecute(string assetPath)
            {
                return SceneField.enabled && assetPath.EndsWith(SCENE_EXT);
            }
        }
        // ================== SceneField ==========================
        private class SceneField : PopupField<Scene>
        {
            public static bool enabled = false;
            
            public SceneField(EventCallback<ChangeEvent<Scene>> callback)
            {
                choices = new();
                foreach (var guid in EUtility.FindGUIDAssets<SceneAsset>())
                    choices.Add(AssetDatabase.GUIDToAssetPath(guid));

                value = SceneManager.GetActiveScene().path;

                this.RegisterValueChangedCallback(callback);

                formatSelectedValueCallback = FormatItem;
                formatListItemCallback = FormatItem;

                enabled = true;
            }

            public SceneField Dispose(EventCallback<ChangeEvent<Scene>> callback)
            {
                enabled = false;

                choices.Clear();
                this.UnregisterCallback(callback);

                return null;
            }

            public void AddItem(string value)
            {
                if (IndexItem(value) < 0)
                    choices.Add(value);
            }

            public void RemoveItem(string value)
            {
                int index = IndexItem(value);
                if (index >= 0)
                    choices.RemoveAt(index);
            }

            public void ReplaceItem(string oldValue, string newValue)
            {
                int index = IndexItem(oldValue);
                if (index >= 0)
                    choices[index] = newValue;
            }

            public int IndexItem(string value)
            {
                for (int i = 0; i < choices.Count; i++)
                    if (choices[i].Equals(value))
                        return i;
                return -1;
            }

            private static string FormatItem(Scene scene) => scene.name;
        }
        // ================== Scene ==========================
        private class Scene : IEquatable<Scene>, IEquatable<string>
        {
            public string path;
            public string name;

            public Scene(string value)
            {
                path = value;
                name = System.IO.Path.GetFileNameWithoutExtension(value);
            }
            public Scene(UScene scene)
            {
                path = scene.path;
                name = scene.name;
            }

            public static implicit operator Scene(string value) => new(value);
            public static implicit operator Scene(UScene value) => new(value);
            
            public static implicit operator string(Scene value) => value.path;

            public bool Equals(string other) => path.Equals(other);
            public bool Equals(Scene other) => other is not null && path.Equals(other.path);

            public override string ToString() => name;
        }
        #endregion
    }
}