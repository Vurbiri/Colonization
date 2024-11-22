//Assets\Vurbiri\Runtime\Web\Yandex\YandexSDK_Methods.cs
using Newtonsoft.Json;
using System;
using System.Collections;

namespace Vurbiri
{
    public partial class YandexSDK
    {
        public IEnumerator Init_Coroutine()
        {
            WaitResult<bool> waitResult;

            yield return (waitResult = InitYsdk());
            if (!waitResult.Result)
            {
                Message.Log("YandexSDK - initialization error!");

                yield break;
            }

            yield return (waitResult = InitPlayer());
            if (!waitResult.Result)
                Message.Log("Player - initialization error!");

            yield return (waitResult = InitLeaderboards());
            if (!waitResult.Result)
                Message.Log("Leaderboards - initialization error!");
        }

        public IEnumerator Authorization_Coroutine(Action<bool> callback)
        {
            ///Message.BannersClear();

            WaitResult<bool> waitResult;

            if (!IsPlayer)
            {
                yield return (waitResult = InitPlayer());
                if (!waitResult.Result)
                {
                    callback?.Invoke(false);
                    yield break;
                }
            }

            if (!IsLogOn)
            {
                yield return (waitResult = LogOn());
                if (!waitResult.Result)
                {
                    callback?.Invoke(false);
                    yield break;
                }
            }

            if (!IsLeaderboard)
                yield return InitLeaderboards();

            callback?.Invoke(true);
        }

        private WaitResult<T> WaitResult<T>(ref WaitResult<T> completion, Action action)
        {
            completion = completion.Recreate();
            action();
            return completion;
        }
        private WaitResult<T> WaitResult<T, U>(ref WaitResult<T> completion, Action<U> action, U value)
        {
            completion = completion.Recreate();
            action(value);
            return completion;
        }
        private WaitResult<T> WaitResult<T, U, V>(ref WaitResult<T> completion, Action<U, V> action, U value1, V value2)
        {
            completion = completion.Recreate();
            action(value1, value2);
            return completion;
        }

        private Return<T> Deserialize<T>(string json) where T : class
        {
            Return<T> result = Return<T>.Empty;
            try
            {
                result = new(JsonConvert.DeserializeObject<T>(json));
            }
            catch (Exception ex)
            {
                Message.Log(ex.Message);
            }

            return result;
        }
    }
}
