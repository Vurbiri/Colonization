using System;
using System.Collections;

public class EmptyStorage : ASaveLoadJsonTo
{
    public override bool IsValid => true;

    public override IEnumerator Initialize_Coroutine(string key, Action<bool> callback)
    {
        callback?.Invoke(false);
        return null;
    }
    public override Return<T> Load<T>(string key) => Return<T>.Empty;
    public override IEnumerator Save_Coroutine(string key, object data, Action<bool> callback)
    { 
        callback?.Invoke(false);
        return null;
    }
}
