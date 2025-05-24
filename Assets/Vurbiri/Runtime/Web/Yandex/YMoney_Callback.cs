using System;

namespace Vurbiri
{
    public partial class YMoney
    {
        private readonly WaitResultSource<bool> _waitEndShowFullscreenAdv = new();
        private readonly WaitResultSource<bool> _waitRewardRewardedVideo = new();
        private readonly WaitResultSource<bool> _waitCloseRewardedVideo = new();

        public void OnEndShowFullscreenAdv(int result) => _waitEndShowFullscreenAdv.SetResult(Convert.ToBoolean(result));
        public void OnRewardRewardedVideo(int result) => _waitRewardRewardedVideo.SetResult(Convert.ToBoolean(result));
        public void OnCloseRewardedVideo(int result) => _waitCloseRewardedVideo.SetResult(Convert.ToBoolean(result));
    }
}
