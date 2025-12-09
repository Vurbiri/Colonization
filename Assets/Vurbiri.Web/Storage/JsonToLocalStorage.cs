using UnityEngine;

namespace Vurbiri.Web
{
    sealed public class JsonToLocalStorage : AJsonToLocal
    {
        public override bool IsValid => Application.platform == RuntimePlatform.WebGLPlayer && UtilityJS.IsStorage();

        public JsonToLocalStorage(string key, MonoBehaviour monoBehaviour) : base(key, monoBehaviour) { }

        protected override string FromStorage() => UtilityJS.GetStorage(_key);
        protected override bool ToStorage() => UtilityJS.SetStorage(_key, Serialize(_saved));
    }
}
