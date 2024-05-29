using System;

public partial class YandexSDK
{
    private WaitResult<bool> _waitEndInitYsdk = new();
    private WaitResult<bool> _waitEndInitPlayer = new();
    private WaitResult<bool> _waitEndLogOn = new();
    private WaitResult<bool> _waitEndInitLeaderboards = new();
    private WaitResult<bool> _waitEndSetScore = new();
    private WaitResult<string> _waitEndGetPlayerResult = new();
    private WaitResult<string> _waitEndGetLeaderboard = new();
    private WaitResult<bool> _waitEndSave = new();
    private WaitResult<string> _waitEndLoad = new();
    private WaitResult<bool> _waitEndCanReview = new();
    private WaitResult<bool> _waitEndRequestReview = new();
    private WaitResult<bool> _waitEndCanShortcut = new();
    private WaitResult<bool> _waitEndCreateShortcut = new();

    public void OnEndInitYsdk(int result) => _waitEndInitYsdk.SetResult(Convert.ToBoolean(result));
    public void OnEndInitPlayer(int result) => _waitEndInitPlayer.SetResult(Convert.ToBoolean(result));
    public void OnEndLogOn(int result) => _waitEndLogOn.SetResult(Convert.ToBoolean(result));
    public void OnEndInitLeaderboards(int result) => _waitEndInitLeaderboards.SetResult(Convert.ToBoolean(result));
    public void OnEndSetScore(int result) => _waitEndSetScore.SetResult(Convert.ToBoolean(result));
    public void OnEndGetPlayerResult(string value) => _waitEndGetPlayerResult.SetResult(value);
    public void OnEndGetLeaderboard(string value) => _waitEndGetLeaderboard.SetResult(value);
    public void OnEndSave(int result) => _waitEndSave.SetResult(Convert.ToBoolean(result));
    public void OnEndLoad(string value) => _waitEndLoad.SetResult(value);
    public void OnEndCanReview(int result) => _waitEndCanReview.SetResult(Convert.ToBoolean(result));
    public void OnEndRequestReview(int result) => _waitEndRequestReview.SetResult(Convert.ToBoolean(result));
    public void OnEndCanShortcut(int result) => _waitEndCanShortcut.SetResult(Convert.ToBoolean(result));
    public void OnEndCreateShortcut(int result) => _waitEndCreateShortcut.SetResult(Convert.ToBoolean(result));
}
