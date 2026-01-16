using Newtonsoft.Json;
using System;
using System.Collections;

namespace Vurbiri.Yandex
{
    public partial class YandexSDK
    {
        public IEnumerator Init_Cn()
        {
            WaitResult<bool> waitResult;

            yield return (waitResult = InitYsdk());
            if (!waitResult.Value)
            {
                Log.Info("[YandexSDK] Initialization error!");
                yield break;
            }

            yield return (waitResult = InitPlayer());
            if (!waitResult.Value)
                Log.Info("[YandexSDK] Player initialization error!");

            yield return (waitResult = InitLeaderboards());
            if (!waitResult.Value)
                Log.Info("[YandexSDK] Leaderboards initialization error!");
        }

        public IEnumerator Authorization_Cn(Out<bool> callback)
        {
            WaitResult<bool> waitResult;

            if (!IsPlayer)
            {
                yield return (waitResult = InitPlayer());
                if (!waitResult.Value)
                {
                    callback?.Set(false);
                    yield break;
                }
            }

            if (!IsLogOn)
            {
                yield return (waitResult = LogOn());
                if (!waitResult.Value)
                {
                    callback?.Set(false);
                    yield break;
                }
            }

            if (!IsLeaderboard)
                yield return InitLeaderboards();

            callback?.Set(true);
        }

        private WaitResult<T> GetResult<T>(ref WaitResultSource<T> completion, Action action)
        {
            completion = completion.Recreate();
            action();
            return completion;
        }
        private WaitResult<T> GetResult<T, U>(ref WaitResultSource<T> completion, Action<U> action, U value)
        {
            completion = completion.Recreate();
            action(value);
            return completion;
        }
        private WaitResult<T> GetResult<T, U, V>(ref WaitResultSource<T> completion, Action<U, V> action, U value1, V value2)
        {
            completion = completion.Recreate();
            action(value1, value2);
            return completion;
        }

        private Return<T> Deserialize<T>(string json)
        {
            try
            {
                if (!string.IsNullOrEmpty(json))
                    return new(JsonConvert.DeserializeObject<T>(json));
            }
            catch (Exception ex)
            {
                Log.Info(ex.Message);
            }

            return Return<T>.Empty;
        }
    }
}
