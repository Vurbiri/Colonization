#if UNITY_EDITOR

namespace VurbiriEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    public static class Utility
    {
        public static T FindAnyPrefab<T>() where T : MonoBehaviour
        {
            string[] guids = AssetDatabase.FindAssets($"t:Prefab", new[] { "Assets" });

            string path; T obj;
            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);
                obj = (AssetDatabase.LoadMainAssetAtPath(path) as GameObject).GetComponent<T>();
                if (obj != null) return obj;
            }

            return default;
        }

        public static List<T> FindPrefabs<T>() where T : MonoBehaviour
        {
            string[] guids = AssetDatabase.FindAssets($"t:Prefab", new[] { "Assets" });
            
            List<T> list = new();
            string path; 
            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);
                if ((AssetDatabase.LoadMainAssetAtPath(path) as GameObject).TryGetComponent<T>(out T obj)) 
                    list.Add(obj);
            }

            return list;
        }

        public static List<T> FindComponentsPrefabs<T>() where T : Component
        {
            string[] guids = AssetDatabase.FindAssets($"t:Prefab", new[] { "Assets" });

            List<T> list = new();
            string path; T[] obj;
            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);
                if((obj = (AssetDatabase.LoadMainAssetAtPath(path) as GameObject).GetComponentsInChildren<T>()) != null)
                    list.AddRange(obj);
            }

            return list;
        }

        public static T FindAnyScriptable<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { "Assets" });

            string path; T obj;
            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);
                obj = AssetDatabase.LoadAssetAtPath<T>(path);
                if (obj != null) return obj;
            }

            return default;
        }

        public static List<T> FindScriptables<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { "Assets" });

            string path; T obj;
            List<T> list = new();
            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);
                obj = AssetDatabase.LoadAssetAtPath<T>(path);
                if (obj != null) 
                    list.Add(obj);
            }

            return list;
        }

        public static void CreateFromPrefab(string path, string name, GameObject parent) => Place(GameObject.Instantiate(Resources.Load(path)) as GameObject, parent, name);

        public static GameObject CreateObject(string name, GameObject parent, params Type[] types)
        {
            GameObject newObject = ObjectFactory.CreateGameObject(name, types);
            Place(newObject, parent, name);

            return newObject;
        }

        private static void Place(GameObject gameObject, GameObject parent, string name)
        {
            gameObject.name = name;
            //gameObject.transform.parent = parent.transform;

            GameObjectUtility.SetParentAndAlign(gameObject, parent);

            // Find location
            //SceneView lastView = SceneView.lastActiveSceneView;
            //gameObject.transform.position = lastView ? lastView.pivot : Vector3.zero;

            // Make sure we place the object in the proper scene, with a relevant name
            StageUtility.PlaceGameObjectInCurrentStage(gameObject);
            GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

            // Record undo, and select
            Undo.RegisterCreatedObjectUndo(gameObject, $"Create Object: {gameObject.name}");
            Selection.activeGameObject = gameObject;

            // For prefabs, let's mark the scene as dirty for saving
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}

#endif