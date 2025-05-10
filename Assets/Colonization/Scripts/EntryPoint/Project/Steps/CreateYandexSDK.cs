//Assets\Colonization\Scripts\EntryPoint\Project\Steps\CreateYandexSDK.cs
using System.Collections;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class CreateYandexSDK : ALoadingStep
    {
        private readonly YandexSDK _ysdk;
        private readonly IEnumerator _init;

        public CreateYandexSDK(DIContainer diContainer, Coroutines coroutine, string lbName) : base("YandexSDK")
        {
            _ysdk = new(coroutine, lbName);
            _init = _ysdk.Init_Cn();

            diContainer.AddInstance(_ysdk);
        }

        public override bool MoveNext() => _init.MoveNext();
    }
}
