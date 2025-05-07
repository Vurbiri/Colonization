//Assets\Colonization\Scripts\Utility\SettingsFile.cs
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class SettingsFile
	{
        public const string FOLDER = "Settings/";

        public static T Load<T>()
        {
            Type type = typeof(T);
            var textAsset = Resources.Load<TextAsset>(string.Concat(FOLDER, type.Name));
            T obj = (T)JsonConvert.DeserializeObject(textAsset.text, type);
            Resources.UnloadAsset(textAsset);
            return obj;
        }
    }
}
