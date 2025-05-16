//Assets\Colonization\Scripts\EntryPoint\Project\Steps\CreateYandexSDK.cs
using System.Collections;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class CreateYandexSDK : ALocalizationLoadingStep
    {
        private readonly YandexSDK _ysdk;

        public CreateYandexSDK(DIContainer diContainer, Coroutines coroutine, string lbName) : base("Yandex")
        {
            _ysdk = new(coroutine, lbName);

            diContainer.AddInstance(_ysdk);
        }

        public override IEnumerator GetEnumerator() => _ysdk.Init_Cn();
    }
}
