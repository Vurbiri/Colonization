//Assets\Vurbiri\Runtime\Utilities\UtilityEditor.cs
#if UNITY_EDITOR

namespace VurbiriEditor
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public static class Utility
    {
        public const string TYPE_PREFAB = "t:Prefab";
        private readonly static string[] ASSET_FOLDERS = new string[] { "Assets" };

        public static T FindAnyPrefab<T>() where T : MonoBehaviour
        {
            foreach (var guid in FindPrefabs())
                if (LoadMainAssetAtGUID(guid).TryGetComponent<T>(out T component))
                    return component;

            return default;
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

            return default;
        }

        public static List<T> FindScriptables<T>() where T : ScriptableObject
        {
            List<T> list = new();
            foreach (var guid in FindAssets<T>())
                if (TryLoadAssetAtGUID<T>(guid, out T scriptable))
                    list.Add(scriptable);

            return list;
        }

        public static string[] FindPrefabs() => AssetDatabase.FindAssets(TYPE_PREFAB, ASSET_FOLDERS);
        public static string[] FindAssets<T>() where T : Object => AssetDatabase.FindAssets($"t:{typeof(T).Name}", ASSET_FOLDERS);
        public static GameObject LoadMainAssetAtGUID(string guid) => ((GameObject)AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guid)));
        public static bool TryLoadAssetAtGUID<T>(string guid, out T obj) where T : Object
        {
            obj = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
            return obj != null;
        }
    }
}
#endif
