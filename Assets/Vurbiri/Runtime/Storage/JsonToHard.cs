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

        protected override WaitResult<string> LoadFromFile_Wait()
        {
            string result = null;
            if (File.Exists(_path))
            {
                using StreamReader sr = new(_path);
                result = sr.ReadToEnd();
            }
            return WaitResult.Instant(result);
        }

        protected override IEnumerator SaveToFile_Cn(WaitResultSource<bool> waitResult)
        {
            bool result = false;
            try
            {
                using StreamWriter sw = new(_path);
                sw.Write(Serialize(_saved));
                result = true;
            }
            catch (Exception ex)
            {
                Log.Info(ex.Message);
            }

            waitResult.Set(result);
            return null;
        }
    }
}
