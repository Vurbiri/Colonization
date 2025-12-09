using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri
{
    public abstract class AJsonToLocal : AStorageOneFile
    {
        protected AJsonToLocal(string key, MonoBehaviour monoBehaviour) : base(key, monoBehaviour) { }

        protected abstract string FromStorage();
        protected abstract bool ToStorage();

        sealed protected override WaitResult<string> LoadFromFile_Wait()
        {
            string result = null;
            try
            {
                result = FromStorage();
            }
            catch (Exception ex)
            {
                Log.Info(ex.Message);
            }

            return WaitResult.Instant(result);
        }

        sealed protected override IEnumerator SaveToFile_Cn(WaitResultSource<bool> waitResult)
        {
            bool result = false;
            try
            {
                result = ToStorage();
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
