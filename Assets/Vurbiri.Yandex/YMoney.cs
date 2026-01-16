using System.Runtime.InteropServices;

namespace Vurbiri.Yandex
{
	public class YMoney
	{
#if !UNITY_EDITOR
		public bool IsInitialize => IsInitializeJS();

		public void ShowBannerAdv() => ShowBannerAdvJS();
		public void HideBannerAdv() => HideBannerAdvJS();
#else
		public bool IsInitialize => true;

		public void ShowBannerAdv() { }
		public void HideBannerAdv() { }
#endif

        private readonly WaitResultSource<bool> _waitEndShowFullScreenAdv = new();
        private readonly WaitResultSource<bool> _waitRewardRewardedVideo = new();
        private readonly WaitResultSource<bool> _waitCloseRewardedVideo = new();

        public void OnEndShowFullScreenAdv(int result) => _waitEndShowFullScreenAdv.Set(result > 0);
        public void OnRewardRewardedVideo(int result) => _waitRewardRewardedVideo.Set(result > 0);
        public void OnCloseRewardedVideo(int result) => _waitCloseRewardedVideo.Set(result > 0);


        [DllImport("__Internal")]
		private static extern bool IsInitializeJS();
		[DllImport("__Internal")]
		private static extern void ShowFullScreenAdvJS();
		[DllImport("__Internal")]
		private static extern void ShowRewardedVideoJS();
		[DllImport("__Internal")]
		private static extern void ShowBannerAdvJS();
		[DllImport("__Internal")]
		private static extern void HideBannerAdvJS();
	}
}
