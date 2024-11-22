//Assets\Vurbiri\Runtime\Utilities\Storage\EmptyStorage.cs
using System;
using System.Collections;

namespace Vurbiri
{
    public class EmptyStorage : ASaveLoadJsonTo
    {
        public override bool IsValid => false;

        public override bool Init(IReadOnlyDIContainer container) => true;

        public override IEnumerator Load_Coroutine(string key, Action<bool> callback)
        {
            callback?.Invoke(false);
            return null;
        }
        public override Return<T> Get<T>(string key) => Return<T>.Empty;
        
        public override bool TryGet<T>(string key, out T value) where T : class
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
