using System;
using System.Collections;
using UnityEngine;

//[DefaultExecutionOrder(-1)]
public partial class YandexSDK : ASingleton<YandexSDK>
{
    [Space]
    [SerializeField] private string _lbName = "lbFindLoner";

#if !UNITY_EDITOR
    public bool IsInitialize => IsInitializeJS();
    public bool IsPlayer => IsPlayerJS();
    public bool IsLogOn => IsLogOnJS();
    public bool IsLeaderboard => IsLeaderboardJS();
    public bool IsDesktop => IsDesktopJS();
    public bool IsMobile => IsMobileJS();

    public string PlayerName => GetPlayerNameJS();
    public string GetPlayerAvatarURL(AvatarSize size) => GetPlayerAvatarURLJS(size.ToString().ToLower());
    public string Lang => GetLangJS();

    public WaitResult<bool> InitYsdk() => WaitResult(ref _waitEndInitYsdk, InitYsdkJS);
    public void LoadingAPI_Ready() => ReadyJS();
    public WaitResult<bool> InitPlayer() => WaitResult(ref _waitEndInitPlayer, InitPlayerJS);

    public WaitResult<bool> LogOn() => WaitResult(ref _waitEndLogOn, LogOnJS);

    public WaitResult<bool> InitLeaderboards() => WaitResult(ref _waitEndInitLeaderboards, InitLeaderboardsJS);
    public WaitResult<Return<PlayerRecord>> GetPlayerResult() 
    {
        WaitResult<Return<PlayerRecord>> wait = new();
        StartCoroutine(GetPlayerResultCoroutine());
        return wait;

        #region Local Function
        IEnumerator GetPlayerResultCoroutine()
        {
            yield return WaitResult(ref _waitEndGetPlayerResult, GetPlayerResultJS, _lbName);
            string json = _waitEndGetPlayerResult.Result;

            if (string.IsNullOrEmpty(json))
                wait.SetResult(Return<PlayerRecord>.Empty);
            else
                wait.SetResult(Storage.Deserialize<PlayerRecord>(json));
        }
        #endregion
    }
    public WaitResult<bool> SetScore(long score) => WaitResult(ref _waitEndSetScore, SetScoreJS, _lbName, score);
    public WaitResult<Return<Leaderboard>> GetLeaderboard(int quantityTop, bool includeUser = false, int quantityAround = 1, AvatarSize size = AvatarSize.Medium)
    {
        WaitResult<Return<Leaderboard>> wait = new();
        StartCoroutine(GetLeaderboardCoroutine());
        return wait;

        #region Local Function
        IEnumerator GetLeaderboardCoroutine()
        {
            _waitEndGetLeaderboard = _waitEndGetLeaderboard.Delete();
            GetLeaderboardJS(_lbName, quantityTop, includeUser, quantityAround, size.ToString().ToLower());
            yield return _waitEndGetLeaderboard;
            string json = _waitEndGetLeaderboard.Result;

            if (string.IsNullOrEmpty(json))
                wait.SetResult(Return<Leaderboard>.Empty);
            else
                wait.SetResult(Storage.Deserialize<Leaderboard>(json));
        }
        #endregion
    }

    public WaitResult<bool> Save(string key, string data) => WaitResult(ref _waitEndSave, SaveJS, key, data);
    public WaitResult<string> Load(string key) => WaitResult(ref _waitEndLoad, LoadJS, key);

    public WaitResult<bool> CanReview() => WaitResult(ref _waitEndCanReview, CanReviewJS);
    public WaitResult<bool> RequestReview() => WaitResult(ref _waitEndRequestReview, RequestReviewJS);

    public WaitResult<bool> CanShortcut() => WaitResult(ref _waitEndCanShortcut, CanShortcutJS);
    public WaitResult<bool> CreateShortcut() => WaitResult(ref _waitEndCreateShortcut, CreateShortcutJS);

#endif

    //public async UniTask<bool> TrySetScore(long points)
    //{
    //    if (!IsLeaderboard || points <= 0)
    //        return false;

    //    var player = await GetPlayerResult();
    //    if (!player.Result)
    //        return false;

    //    if (player.Value.Score >= points)
    //        return false;

    //    return await SetScore(points);
    //}

    private WaitResult<T> WaitResult<T>(ref WaitResult<T> completion, Action action)
    {
        completion = completion.Delete();
        action();
        return completion;
    }
    private WaitResult<T> WaitResult<T, U>(ref WaitResult<T> completion, Action<U> action, U value)
    {
        completion = completion.Delete();
        action(value);
        return completion;
    }
    private WaitResult<T> WaitResult<T, U, V>(ref WaitResult<T> completion, Action<U, V> action, U value1, V value2)
    {
        completion = completion.Delete();
        action(value1, value2);
        return completion;
    }
}

