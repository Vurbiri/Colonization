//Assets\Vurbiri\Runtime\Utilities\UtilityEditor.cs
#if UNITY_EDITOR

namespace VurbiriEditor
{
    using System.Collections.Generic;
    using UnityEditor;
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
    }
}

#endif
