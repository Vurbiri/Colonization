using System;
using System.Collections;
using System.Collections.Generic;

public class JsonToYandex : ASaveLoadJsonTo
{
    private string _key;
    private readonly YandexSDK _ysdk;

    public override bool IsValid => _ysdk.IsLogOn;

    public JsonToYandex()
    {
        _ysdk = YandexSDK.InstanceF;
    }

    public override IEnumerator Initialize_Coroutine(string key, Action<bool> callback)
    {
        _key = key;
        
        WaitResult<string> waitResult;
        string json;

        yield return (waitResult = _ysdk.Load(_key));
        json = waitResult.Result;
        
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

    public override IEnumerator Save_Coroutine(string key, object data, Action<bool> callback)
    {
        bool result = SaveToMemory(key, data);
        if (!(result && _dictModified))
        {
            callback?.Invoke(false);
            yield break;
        }

        yield return SaveToFileCoroutine();

        #region Local Function
        IEnumerator SaveToFileCoroutine()
        {
            WaitResult<bool> waitResult;
            yield return (waitResult = _ysdk.Save(_key, Serialize(_saved)));
            _dictModified = !waitResult.Result;
            callback?.Invoke(waitResult.Result);
        }
        #endregion
    }
}
