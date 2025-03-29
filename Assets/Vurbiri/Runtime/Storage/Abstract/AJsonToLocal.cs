//Assets\Vurbiri\Runtime\Storage\Abstract\AJsonToLocal.cs
using System;
using UnityEngine;

namespace Vurbiri
{
    public abstract class AJsonToLocal : AStorageOneFile
    {
        protected AJsonToLocal(string key, MonoBehaviour monoBehaviour) : base(key, monoBehaviour)
        {
        }

        protected abstract string GetStorage();
        protected abstract bool SetStorage();

        sealed protected override WaitResult<string> LoadFromFile_Wait()
        {
            WaitResultSource<string> waitResult = new();
            try
            {
                waitResult.SetResult(GetStorage());
            }
            catch (Exception ex)
            {
                waitResult.SetResult(null);
                Message.Log(ex.Message);
            }

            return waitResult;
        }

        sealed protected override WaitResult<bool> SaveToFile_Wait()
        {
            WaitResultSource<bool> waitResult = new();

            try
            {
                waitResult.SetResult(SetStorage());
            }
            catch (Exception ex)
            {
                waitResult.SetResult(false);
                Message.Log(ex.Message);
            }

            return waitResult;
        }
    }
}
