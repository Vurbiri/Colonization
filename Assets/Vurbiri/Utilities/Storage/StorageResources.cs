using UnityEngine;

namespace Vurbiri
{
    public static class StorageResources
    {
        public static Return<T> LoadFromJson<T>(string path) where T : class
        {
            string json = Resources.Load<TextAsset>(path).text;
            return Storage.Deserialize<T>(json);
        }
    }
}
