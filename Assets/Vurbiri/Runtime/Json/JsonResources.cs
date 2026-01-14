using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri
{
    public static class JsonResources
    {
        public static bool TryLoad<T>(string path, out T obj, bool logWarning = true)
        {
            try
            {
                obj = (T)Load(path, typeof(T));
                return true;
            }
            catch (Exception ex)
            {
                if(logWarning) Log.Warning($"[Error::Storage] Failed to load object {typeof(T).Name} on the path {path}.\n".Concat(ex.Message));
                obj = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Load<T>(string path) => (T)Load(path, typeof(T));
        public static object Load(string path, Type type)
        {
            var textAsset = Resources.Load<TextAsset>(path);
            var obj = JsonConvert.DeserializeObject(textAsset.text, type, settings: null);
            Resources.UnloadAsset(textAsset);
            return obj;
        }
    }
}
