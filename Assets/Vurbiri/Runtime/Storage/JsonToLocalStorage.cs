//Assets\Vurbiri\Runtime\Storage\JsonToLocalStorage.cs
using UnityEngine;

namespace Vurbiri
{
    sealed public class JsonToLocalStorage : AJsonToLocal
    {
        public override bool IsValid => UtilityJS.IsStorage();

        public JsonToLocalStorage(MonoBehaviour monoBehaviour) : base(monoBehaviour)
        {
        }

        protected override string GetStorage() => UtilityJS.GetStorage(_key);
        protected override bool SetStorage() => UtilityJS.SetStorage(_key, Serialize(_saved));
    }
}
