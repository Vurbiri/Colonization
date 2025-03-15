//Assets\Vurbiri\Runtime\Storage\JsonToCookies.cs
using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri
{
    sealed public class JsonToCookies : ASaveLoadJsonTo
    {
        public override bool IsValid => UtilityJS.IsCookies();

        public override bool Init(IReadOnlyDIContainer container)
        {
            Init(container.Get<Coroutines>());
            return UtilityJS.IsCookies();
        }

        public override IEnumerator Load_Cn(string key, Action<bool> callback)
        {
            _key = key;

            string json;
            try
            {
                json = UtilityJS.GetCookies(_key);
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
            WaitResult<bool> waitResult = new();

            try
            {
                waitResult.SetResult(UtilityJS.SetCookies(_key, Serialize(_saved)));
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
