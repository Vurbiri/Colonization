using System;
using UnityEditor;
using UnityEngine;

namespace Vurbiri
{
    public abstract class AGetOrCreateScriptableObject<T> : ScriptableObject, IDisposable where T : ScriptableObject
    {
        protected static T GetOrCreateSelf(string assetName, string assetPath)
        {
            var self = Resources.Load<T>(assetName);
            if (self == null)
            {
                self = CreateInstance<T>();
                AssetDatabase.CreateAsset(self, assetPath);
                AssetDatabase.SaveAssets();
            }
            return self;
        }

        public virtual void Dispose()
        {
            Debug.Log($"Dispose {name}");
            Resources.UnloadAsset(this);
        }
    }
}
