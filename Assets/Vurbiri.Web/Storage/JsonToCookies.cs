using UnityEngine;

namespace Vurbiri.Web
{
    sealed public class JsonToCookies : AJsonToLocal
    {
        public override bool IsValid => Application.platform == RuntimePlatform.WebGLPlayer && UtilityJS.IsCookies();

        public JsonToCookies(string key, MonoBehaviour monoBehaviour) : base(key, monoBehaviour)
        {
        }

        protected override string FromStorage() => UtilityJS.GetCookies(_key);
        protected override bool ToStorage() => UtilityJS.SetCookies(_key, Serialize(_saved));
    }
}
