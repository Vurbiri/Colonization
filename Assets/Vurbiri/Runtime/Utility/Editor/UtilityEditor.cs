//Assets\Vurbiri\Runtime\Utility\Editor\UtilityEditor.cs
#if UNITY_EDITOR

namespace Vurbiri
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public static class EUtility
    {
        public const string TYPE_PREFAB = "t:Prefab";
        public readonly static string[] ASSET_FOLDERS = new string[] { "Assets" };

        public static void SetObject<T>(ref T obj, string name = null) where T : Component
        {
            if (obj != null) return;

            obj = string.IsNullOrEmpty(name) ? Object.FindAnyObjectByType<T>(FindObjectsInactive.Include) : FindObjectByName<T>(name);
            if (obj == null)
                LogErrorFind<T>("object", name);
        }

        public static void SetPrefab<T>(ref T obj, string name = null) where T : MonoBehaviour
        {
            if (obj != null) return;

            obj = string.IsNullOrEmpty(name) ? FindAnyPrefab<T>() : FindAnyPrefab<T>(name);
            if (obj == null)
                LogErrorFind<T>("prefab", name);
        }

        public static void SetScriptable<T>(ref T obj, string name = null) where T : ScriptableObject
        {
            if (obj != null) return;
            obj = string.IsNullOrEmpty(name) ? FindAnyScriptable<T>() : FindAnyScriptable<T>(name);
            if (obj == null)
                LogErrorFind<T>("scriptable", name);
        }

        public static void SetAsset<T>(ref T obj, string name = null) where T : Object
        {
            if (obj != null) return;

            obj = string.IsNullOrEmpty(name) ? FindAnyAsset<T>() : FindAnyAsset<T>(name);
            if (obj == null)
                LogErrorFind<T>("asset", name);
        }

        // ********************************************

        public static T GetComponentInChildren<T>(this Component self, string name) where T : Component
        {
            return self.GetComponentsInChildren<T>().Where(t => t.gameObject.name == name).First();
        }

        // ********************************************

        public static T FindObjectByName<T>(string name) where T : Component
        {
            return Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(t => t.gameObject.name == name).First();
        }

        public static T FindAnyPrefab<T>() where T : MonoBehaviour
        {
            foreach (var guid in FindPrefabs())
                if (LoadMainAssetAtGUID(guid).TryGetComponent<T>(out T component))
                    return component;

            return null;
        }
        public static T FindAnyPrefab<T>(string name) where T : MonoBehaviour
        {
            foreach (var guid in FindPrefabs(name))
                if (LoadMainAssetAtGUID(guid).TryGetComponent<T>(out T component))
                    return component;

            return null;
        }
        public static GameObject FindAnyPrefab(string name)
        {
            foreach (var guid in FindPrefabs(name))
               return LoadMainAssetAtGUID(guid);
            
            return null;
        }

        public static List<T> FindPrefabs<T>() where T : MonoBehaviour
        {
            List<T> list = new();
            foreach (var guid in FindPrefabs())
                if (LoadMainAssetAtGUID(guid).TryGetComponent<T>(out T component))
                    list.Add(component);

            return list;
        }

        public static List<T> FindComponentsPrefabs<T>() where T : Component
        {
            List<T> list = new(); T[] components;
            foreach (var guid in FindPrefabs())
                if ((components = LoadMainAssetAtGUID(guid).GetComponentsInChildren<T>()) != null)
                    list.AddRange(components);

            return list;
        }

        public static T FindAnyScriptable<T>() where T : ScriptableObject
        {
            foreach (var guid in FindAssets<T>())
                if (TryLoadAssetAtGUID<T>(guid, out T scriptable))
                    return scriptable;

            return null;
        }
        public static T FindAnyScriptable<T>(string name) where T : ScriptableObject
        {
            foreach (var guid in FindAssets<T>(name))
                if (TryLoadAssetAtGUID<T>(guid, out T scriptable))
                    return scriptable;

            return null;
        }

        public static List<T> FindScriptables<T>() where T : ScriptableObject
        {
            List<T> list = new();
            foreach (var guid in FindAssets<T>())
                if (TryLoadAssetAtGUID<T>(guid, out T scriptable))
                    list.Add(scriptable);

            return list;
        }

        public static T FindAnyAsset<T>() where T : Object
        {
            foreach (var guid in FindAssets<T>())
                if (TryLoadAssetAtGUID<T>(guid, out T asset))
                    return asset;

            return null;
        }
        public static T FindAnyAsset<T>(string name) where T : Object
        {
            foreach (var guid in FindAssets<T>(name))
                if (TryLoadAssetAtGUID<T>(guid, out T asset))
                    return asset;

            return null;
        }

        // ********************************************

        public static string[] FindPrefabs() => AssetDatabase.FindAssets(TYPE_PREFAB, ASSET_FOLDERS);
        public static string[] FindPrefabs(string name) => AssetDatabase.FindAssets($"{name} {TYPE_PREFAB}", ASSET_FOLDERS);
        public static string[] FindAssets<T>() where T : Object => AssetDatabase.FindAssets($"t:{typeof(T).Name}", ASSET_FOLDERS);
        public static string[] FindAssets<T>(string name) where T : Object => AssetDatabase.FindAssets($"{name} t:{typeof(T).Name}", ASSET_FOLDERS);
        public static GameObject LoadMainAssetAtGUID(string guid) => ((GameObject)AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guid)));
        public static bool TryLoadAssetAtGUID<T>(string guid, out T obj) where T : Object
        {
            obj = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
            return obj != null;
        }

        public static bool IsStartEditor => EditorApplication.timeSinceStartup < 120f;

        private static void LogErrorFind<T>(string type, string name)
        {
            Debug.LogError($"<color=orange> Unable to find the {type} <b>{typeof(T).Name}</b> {(string.IsNullOrEmpty(name) ? string.Empty : $"\"{name}\"")}</color>");
        }

    }
}
#endif
