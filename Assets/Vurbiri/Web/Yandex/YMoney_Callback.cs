using System;

namespace Vurbiri.Colonization
{
    public partial class YMoney
    {
        private readonly WaitResult<bool> _waitEndShowFullscreenAdv = new();
        private readonly WaitResult<bool> _waitRewardRewardedVideo = new();
        private readonly WaitResult<bool> _waitCloseRewardedVideo = new();

        public void OnEndShowFullscreenAdv(int result) => _waitEndShowFullscreenAdv.SetResult(Convert.ToBoolean(result));
        public void OnRewardRewardedVideo(int result) => _waitRewardRewardedVideo.SetResult(Convert.ToBoolean(result));
        public void OnCloseRewardedVideo(int result) => _waitCloseRewardedVideo.SetResult(Convert.ToBoolean(result));
    }
}
