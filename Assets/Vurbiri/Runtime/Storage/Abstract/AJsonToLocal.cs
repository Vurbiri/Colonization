using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri
{
    public abstract class AJsonToLocal : AStorageOneFile
    {
        protected AJsonToLocal(string key, MonoBehaviour monoBehaviour) : base(key, monoBehaviour) { }

        protected abstract string GetStorage();
        protected abstract bool SetStorage();

        sealed protected override IEnumerator LoadFromFile_Cn()
        {
            try
            {
                _outputJson = GetStorage();
            }
            catch (Exception ex)
            {
                _outputJson = null;
                Log.Info(ex.Message);
            }

            return null;
        }

        sealed protected override IEnumerator SaveToFile_Cn()
        {
            try
            {
                _outputResult = SetStorage();
            }
            catch (Exception ex)
            {
                _outputResult = false;
                Log.Info(ex.Message);
            }

            return null;
        }
    }
}
