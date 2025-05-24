using UnityEngine;

namespace Vurbiri
{
    sealed public class JsonToLocalStorage : AJsonToLocal
    {
        public override bool IsValid => UtilityJS.IsStorage();

        public JsonToLocalStorage(string key, MonoBehaviour monoBehaviour) : base(key, monoBehaviour)
        {
        }

        protected override string GetStorage() => UtilityJS.GetStorage(_key);
        protected override bool SetStorage() => UtilityJS.SetStorage(_key, Serialize(_saved));
    }
}
