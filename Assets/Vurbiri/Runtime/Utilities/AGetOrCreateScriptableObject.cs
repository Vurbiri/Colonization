//Assets\Vurbiri\Runtime\Utilities\AGetOrCreateScriptableObject.cs
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Vurbiri
{
    public abstract class AGetOrCreateScriptableObject<T> : ScriptableObjectDisposable where T : ScriptableObject
    {
#if UNITY_EDITOR
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
#endif
    }
}
