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

    protected override WaitResult<bool> SaveToFile_Wait()
    {
        WaitResult<bool> waitResult = new();
        waitResult.SetResult(false);
        return waitResult;
    }
}
