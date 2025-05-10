//Assets\Colonization\Scripts\EntryPoint\Project\Steps\CreateYandexSDK.cs
using System.Collections;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class CreateYandexSDK : ALoadingStep
    {
        private readonly YandexSDK _ysdk;

        public CreateYandexSDK(DIContainer diContainer, Coroutines coroutine, string lbName) : base("YandexSDK...")
        {
            _ysdk = new(coroutine, lbName);

            diContainer.AddInstance(_ysdk);
        }

        public override IEnumerator GetEnumerator() => _ysdk.Init_Cn();
    }
}
