using System;

namespace Vurbiri.Yandex
{
    public partial class YandexSDK
    {
        private WaitResultSource<bool> _waitEndInitYsdk = new();
        private WaitResultSource<bool> _waitEndInitPlayer = new();
        private WaitResultSource<bool> _waitEndLogOn = new();
        private WaitResultSource<bool> _waitEndInitLeaderboards = new();
        private WaitResultSource<bool> _waitEndSetScore = new();
        private WaitResultSource<string> _waitEndGetPlayerResult = new();
        private WaitResultSource<string> _waitEndGetLeaderboard = new();
        private WaitResultSource<bool> _waitEndSave = new();
        private WaitResultSource<string> _waitEndLoad = new();
        private WaitResultSource<bool> _waitEndCanReview = new();
        private WaitResultSource<bool> _waitEndRequestReview = new();
        private WaitResultSource<bool> _waitEndCanShortcut = new();
        private WaitResultSource<bool> _waitEndCreateShortcut = new();

        public void OnEndInitYsdk(int result) => _waitEndInitYsdk.Set(Convert.ToBoolean(result));
        public void OnEndInitPlayer(int result) => _waitEndInitPlayer.Set(Convert.ToBoolean(result));
        public void OnEndLogOn(int result) => _waitEndLogOn.Set(Convert.ToBoolean(result));
        public void OnEndInitLeaderboards(int result) => _waitEndInitLeaderboards.Set(Convert.ToBoolean(result));
        public void OnEndSetScore(int result) => _waitEndSetScore.Set(Convert.ToBoolean(result));
        public void OnEndGetPlayerResult(string value) => _waitEndGetPlayerResult.Set(value);
        public void OnEndGetLeaderboard(string value) => _waitEndGetLeaderboard.Set(value);
        public void OnEndSave(int result) => _waitEndSave.Set(Convert.ToBoolean(result));
        public void OnEndLoad(string value) => _waitEndLoad.Set(value);
        public void OnEndCanReview(int result) => _waitEndCanReview.Set(Convert.ToBoolean(result));
        public void OnEndRequestReview(int result) => _waitEndRequestReview.Set(Convert.ToBoolean(result));
        public void OnEndCanShortcut(int result) => _waitEndCanShortcut.Set(Convert.ToBoolean(result));
        public void OnEndCreateShortcut(int result) => _waitEndCreateShortcut.Set(Convert.ToBoolean(result));
    }
}
