//Assets\Vurbiri\Runtime\Web\Yandex\YandexSDK.cs
#if !UNITY_EDITOR
using System.Collections;
#endif


namespace Vurbiri
{
    public partial class YandexSDK
    {
        private readonly string _lbName = "lbColonization";
        private readonly Coroutines _coroutines;

        public YandexSDK(IReadOnlyDIContainer container, string lbName = null)
        {
            _coroutines = container.Get<Coroutines>();
            if(!string.IsNullOrEmpty(lbName))
                _lbName = lbName;
        }
    }

#if !UNITY_EDITOR
    public partial class YandexSDK
    {
        public bool IsInitialize => IsInitializeJS();
        public bool IsPlayer => IsPlayerJS();
        public bool IsLogOn => IsLogOnJS();
        public bool IsLeaderboard => IsLeaderboardJS();
        public bool IsDesktop => IsDesktopJS();
        public bool IsMobile => IsMobileJS();

        public string PlayerName => GetPlayerNameJS();
        public string GetPlayerAvatarURL(AvatarSize size) => GetPlayerAvatarURLJS(size.ToString().ToLower());
        public string Lang => GetLangJS();

        private WaitResult<bool> InitYsdk() => WaitResult(ref _waitEndInitYsdk, InitYsdkJS);
        public void LoadingAPI_Ready() => ReadyJS();
        public WaitResult<bool> InitPlayer() => WaitResult(ref _waitEndInitPlayer, InitPlayerJS);

        public WaitResult<bool> LogOn() => WaitResult(ref _waitEndLogOn, LogOnJS);

        public WaitResult<bool> InitLeaderboards() => WaitResult(ref _waitEndInitLeaderboards, InitLeaderboardsJS);
        public WaitResult<Return<PlayerRecord>> GetPlayerResult() 
        {
            WaitResult<Return<PlayerRecord>> wait = new();
            _coroutines.Run(GetPlayerResult_Coroutine(wait));
            return wait;

            #region Local: GetPlayerResult_Coroutine()
            //============================================
            IEnumerator GetPlayerResult_Coroutine(WaitResult<Return<PlayerRecord>> wait)
            {
                yield return WaitResult(ref _waitEndGetPlayerResult, GetPlayerResultJS, _lbName);
                string json = _waitEndGetPlayerResult.Result;

                if (string.IsNullOrEmpty(json))
                    wait.SetResult(Return<PlayerRecord>.Empty);
                else
                    wait.SetResult(Deserialize<PlayerRecord>(json));
            }
            #endregion
        }
        public WaitResult<bool> SetScore(long score) => WaitResult(ref _waitEndSetScore, SetScoreJS, _lbName, score);
        public WaitResult<Return<Leaderboard>> GetLeaderboard(int quantityTop, bool includeUser = false, int quantityAround = 1, AvatarSize size = AvatarSize.Medium)
        {
            WaitResult<Return<Leaderboard>> wait = new();
            _coroutines.Run(GetLeaderboardCoroutine(wait));
            return wait;

            #region Local Function
            IEnumerator GetLeaderboardCoroutine(WaitResult<Return<Leaderboard>> wait)
            {
                _waitEndGetLeaderboard = _waitEndGetLeaderboard.Delete();
                GetLeaderboardJS(_lbName, quantityTop, includeUser, quantityAround, size.ToString().ToLower());
                yield return _waitEndGetLeaderboard;
                string json = _waitEndGetLeaderboard.Result;

                if (string.IsNullOrEmpty(json))
                    wait.SetResult(Return<Leaderboard>.Empty);
                else
                    wait.SetResult(Deserialize<Leaderboard>(json));
            }
            #endregion
        }

        public WaitResult<bool> Save(string key, string data) => WaitResult(ref _waitEndSave, SaveJS, key, data);
        public WaitResult<string> Load(string key) => WaitResult(ref _waitEndLoad, LoadJS, key);

        public WaitResult<bool> CanReview() => WaitResult(ref _waitEndCanReview, CanReviewJS);
        public WaitResult<bool> RequestReview() => WaitResult(ref _waitEndRequestReview, RequestReviewJS);

        public WaitResult<bool> CanShortcut() => WaitResult(ref _waitEndCanShortcut, CanShortcutJS);
        public WaitResult<bool> CreateShortcut() => WaitResult(ref _waitEndCreateShortcut, CreateShortcutJS);
    }
#endif
}

