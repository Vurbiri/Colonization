//Assets\Vurbiri\Runtime\Web\Yandex\YMoney_DllImport.cs
using System.Runtime.InteropServices;

namespace Vurbiri
{
    public partial class YMoney
    {

        [DllImport("__Internal")]
        private static extern bool IsInitializeJS();
        [DllImport("__Internal")]
        private static extern void ShowFullscreenAdvJS();
        [DllImport("__Internal")]
        private static extern void ShowRewardedVideoJS();
        [DllImport("__Internal")]
        private static extern void ShowBannerAdvJS();
        [DllImport("__Internal")]
        private static extern void HideBannerAdvJS();
    }
}
