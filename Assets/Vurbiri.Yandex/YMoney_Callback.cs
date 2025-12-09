using System;

namespace Vurbiri.Yandex
{
    public partial class YMoney
    {
        private readonly WaitResultSource<bool> _waitEndShowFullscreenAdv = new();
        private readonly WaitResultSource<bool> _waitRewardRewardedVideo = new();
        private readonly WaitResultSource<bool> _waitCloseRewardedVideo = new();

        public void OnEndShowFullscreenAdv(int result) => _waitEndShowFullscreenAdv.Set(Convert.ToBoolean(result));
        public void OnRewardRewardedVideo(int result) => _waitRewardRewardedVideo.Set(Convert.ToBoolean(result));
        public void OnCloseRewardedVideo(int result) => _waitCloseRewardedVideo.Set(Convert.ToBoolean(result));
    }
}
