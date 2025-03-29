//Assets\Vurbiri\Runtime\Storage\JsonToCookies.cs
using UnityEngine;

namespace Vurbiri
{
    sealed public class JsonToCookies : AJsonToLocal
    {
        public override bool IsValid => UtilityJS.IsCookies();

        public JsonToCookies(string key, MonoBehaviour monoBehaviour) : base(key, monoBehaviour)
        {
        }

        protected override string GetStorage() => UtilityJS.GetCookies(_key);
        protected override bool SetStorage() => UtilityJS.SetCookies(_key, Serialize(_saved));
    }
}
