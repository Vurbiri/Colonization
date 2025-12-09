#if !UNITY_EDITOR
using System.Collections;
#endif

using UnityEngine;

namespace Vurbiri.Yandex
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

        private WaitResult<bool> InitYsdk() => GetResult(ref _waitEndInitYsdk, InitYsdkJS);
        public void LoadingAPI_Ready() => ReadyJS();
        public WaitResult<bool> InitPlayer() => GetResult(ref _waitEndInitPlayer, InitPlayerJS);

        public WaitResult<bool> LogOn() => GetResult(ref _waitEndLogOn, LogOnJS);

        public WaitResult<bool> InitLeaderboards() => GetResult(ref _waitEndInitLeaderboards, InitLeaderboardsJS);
        public WaitResult<Return<PlayerRecord>> GetPlayerResult() 
        {
            WaitResultSource<Return<PlayerRecord>> wait = new();
            _mono.StartCoroutine(GetPlayerResult_Cn(wait));
            return wait;

            #region Local: GetPlayerResult_Cn()
            //============================================
            IEnumerator GetPlayerResult_Cn(WaitResultSource<Return<PlayerRecord>> wait)
            {
                yield return GetResult(ref _waitEndGetPlayerResult, GetPlayerResultJS, _lbName);
                string json = _waitEndGetPlayerResult.Value;

                if (string.IsNullOrEmpty(json))
                    wait.Set(Return<PlayerRecord>.Empty);
                else
                    wait.Set(Deserialize<PlayerRecord>(json));
            }
            #endregion
        }
        public WaitResult<bool> SetScore(long score) => GetResult(ref _waitEndSetScore, SetScoreJS, _lbName, score);
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
                    wait.Set(Return<Leaderboard>.Empty);
                else
                    wait.Set(Deserialize<Leaderboard>(json));
            }
            #endregion
        }

        public IEnumerator Save(string key, string data, WaitResultSource<bool> waitResult)
        {
            _waitEndSave = waitResult;
            SaveJS(key, data);
            return waitResult;
        }
        public WaitResult<string> Load(string key) => GetResult(ref _waitEndLoad, LoadJS, key);

        public WaitResult<bool> CanReview() => GetResult(ref _waitEndCanReview, CanReviewJS);
        public WaitResult<bool> RequestReview() => GetResult(ref _waitEndRequestReview, RequestReviewJS);

        public WaitResult<bool> CanShortcut() => GetResult(ref _waitEndCanShortcut, CanShortcutJS);
        public WaitResult<bool> CreateShortcut() => GetResult(ref _waitEndCreateShortcut, CreateShortcutJS);
    }
#endif
}

