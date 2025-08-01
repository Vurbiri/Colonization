#if !UNITY_EDITOR
using System.Collections;
#endif

using UnityEngine;

namespace Vurbiri
{
    public partial class YandexSDK
    {
        private readonly string _lbName = "lbColonization";
        private readonly MonoBehaviour _mono;

        public YandexSDK(MonoBehaviour mono, string lbName)
        {
            _mono = mono;
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
            WaitResultSource<Return<PlayerRecord>> wait = new();
            _mono.StartCoroutine(GetPlayerResult_Cn(wait));
            return wait;

            #region Local: GetPlayerResult_Cn()
            //============================================
            IEnumerator GetPlayerResult_Cn(WaitResultSource<Return<PlayerRecord>> wait)
            {
                yield return WaitResult(ref _waitEndGetPlayerResult, GetPlayerResultJS, _lbName);
                string json = _waitEndGetPlayerResult.Value;

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
            WaitResultSource<Return<Leaderboard>> wait = new();
            _mono.StartCoroutine(GetLeaderboardCoroutine(wait));
            return wait;

            #region Local Function
            IEnumerator GetLeaderboardCoroutine(WaitResultSource<Return<Leaderboard>> wait)
            {
                _waitEndGetLeaderboard = _waitEndGetLeaderboard.Recreate();
                GetLeaderboardJS(_lbName, quantityTop, includeUser, quantityAround, size.ToString().ToLower());
                yield return _waitEndGetLeaderboard;
                string json = _waitEndGetLeaderboard.Value;

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

