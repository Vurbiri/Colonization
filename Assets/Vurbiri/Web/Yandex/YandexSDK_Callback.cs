using System;

namespace Vurbiri
{
    public partial class YandexSDK
    {
        private readonly WaitResult<bool> _waitEndInitYsdk = new();
        private readonly WaitResult<bool> _waitEndInitPlayer = new();
        private readonly WaitResult<bool> _waitEndLogOn = new();
        private readonly WaitResult<bool> _waitEndInitLeaderboards = new();
        private readonly WaitResult<bool> _waitEndSetScore = new();
        private readonly WaitResult<string> _waitEndGetPlayerResult = new();
        private readonly WaitResult<string> _waitEndGetLeaderboard = new();
        private readonly WaitResult<bool> _waitEndSave = new();
        private readonly WaitResult<string> _waitEndLoad = new();
        private readonly WaitResult<bool> _waitEndCanReview = new();
        private readonly WaitResult<bool> _waitEndRequestReview = new();
        private readonly WaitResult<bool> _waitEndCanShortcut = new();
        private readonly WaitResult<bool> _waitEndCreateShortcut = new();

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
}
