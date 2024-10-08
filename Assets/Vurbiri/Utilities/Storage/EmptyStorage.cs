using System;
using System.Collections;

namespace Vurbiri
{
    public class EmptyStorage : ASaveLoadJsonTo
    {
        public override bool IsValid => true;

        public override IEnumerator Init_Coroutine(string key, Action<bool> callback)
        {
            callback?.Invoke(false);
            return null;
        }
        public override Return<T> Load<T>(string key) => Return<T>.Empty;
        
        public override bool TryLoad<T>(string key, out T value) where T : class
        {
            value = null;
            return false;
        }

        protected override WaitResult<bool> SaveToFile_Wait()
        {
            WaitResult<bool> waitResult = new();
            waitResult.SetResult(false);
            return waitResult;
        }
    }
}
