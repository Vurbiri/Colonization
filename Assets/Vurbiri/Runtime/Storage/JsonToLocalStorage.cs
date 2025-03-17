//Assets\Vurbiri\Runtime\Storage\JsonToLocalStorage.cs
using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri
{
    sealed public class JsonToLocalStorage : ASaveLoadJsonTo
    {
        public override bool IsValid => UtilityJS.IsStorage();

        public override bool Init(IReadOnlyDIContainer container)
        {
            Init(container.Get<Coroutines>());
            return UtilityJS.IsStorage();
        }

        public override IEnumerator Load_Cn(string key, Action<bool> callback)
        {
            _key = key;

            string json;

            try
            {
                json = UtilityJS.GetStorage(_key);
            }
            catch (Exception ex)
            {
                json = null;
                Message.Log(ex.Message);
            }

            if (!string.IsNullOrEmpty(json))
            {
                Return<Dictionary<string, string>> d = Deserialize<Dictionary<string, string>>(json);

                if (d.Result)
                {
                    _saved = d.Value;
                    callback?.Invoke(true);
                    yield break;
                }
            }

            _saved = new();
            callback?.Invoke(false);
        }

        protected override WaitResult<bool> SaveToFile_Wait()
        {
            WaitResultSource<bool> waitResult = new();

            try
            {
                waitResult.SetResult(UtilityJS.SetStorage(_key, Serialize(_saved)));
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
