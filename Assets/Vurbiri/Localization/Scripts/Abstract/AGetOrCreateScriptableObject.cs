using UnityEditor;
using UnityEngine;

namespace Vurbiri.Localization
{
    public abstract class AGetOrCreateScriptableObject<T> : ScriptableObject where T : ScriptableObject
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
    }
}
