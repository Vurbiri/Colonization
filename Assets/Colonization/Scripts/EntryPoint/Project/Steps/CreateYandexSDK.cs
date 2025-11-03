using System.Collections;
using UnityEngine;
using Vurbiri.Yandex;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class CreateYandexSDK : ALocalizationLoadingStep
    {
        private readonly YandexSDK _ysdk;

        public CreateYandexSDK(ProjectContent content, MonoBehaviour mono, string lbName) : base("YandexStep")
        {
            content.ysdk = _ysdk = new(mono, lbName);
        }

        public override IEnumerator GetEnumerator() => _ysdk.Init_Cn();
    }
}
