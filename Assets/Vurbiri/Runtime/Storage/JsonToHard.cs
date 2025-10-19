using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Vurbiri
{
    sealed public class JsonToHard : AStorageOneFile
    {
        private readonly string _path;

        public JsonToHard(string key, MonoBehaviour monoBehaviour) : base(string.Empty, monoBehaviour) => _path = Path.Combine(Application.persistentDataPath, key);

#if UNITY_EDITOR
        public override bool IsValid => Application.platform == RuntimePlatform.WindowsEditor;
#else
        public override bool IsValid => Application.platform == RuntimePlatform.WindowsPlayer;
#endif

        protected override IEnumerator SaveToFile_Cn()
        {
            try
            {
                using StreamWriter sw = new(_path);
                sw.Write(Serialize(_saved));
                _outputResult = true;
            }
            catch (Exception ex)
            {
                _outputResult = false;
                Log.Info(ex.Message);
            }

            return null;
        }

        protected override IEnumerator LoadFromFile_Cn()
        {
            _outputJson = string.Empty;
            if (File.Exists(_path))
            {
                using StreamReader sr = new(_path);
                _outputJson = sr.ReadToEnd();
            }
            return null;
        }
    }
}
