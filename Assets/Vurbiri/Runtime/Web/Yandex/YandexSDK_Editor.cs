//Assets\Vurbiri\Runtime\Web\Yandex\YandexSDK_Editor.cs
#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Vurbiri
{
    public partial class YandexSDK
    {
        private readonly bool _isInitialize = true;
        private readonly bool _isLeaderboard = true;
        private readonly bool _isPlayer = true;

        public bool IsDesktop = true;
        public bool IsInitialize = true;
        public string PlayerName = "Best of the Best";
        public bool IsLogOn = true;
        public string Lang = "ru";
        public bool IsPlayer => IsInitialize && _isPlayer;
        public bool IsLeaderboard => IsLogOn && _isLeaderboard;

        public WaitResult<bool> InitYsdk() => new WaitResultSource<bool>(_isInitialize);
        public void LoadingAPI_Ready() { }
        public WaitResult<bool> InitPlayer() => new WaitResultSource<bool>(_isPlayer);
        public WaitResult<bool> LogOn()
        {
            IsLogOn = true;
            return new WaitResultSource<bool>(IsLogOn); ;
        }
        public WaitResult<bool> InitLeaderboards() => new WaitResultSource<bool>(IsLeaderboard);
        public string GetPlayerAvatarURL(AvatarSize size) => string.Empty;

        public WaitResultSource<Return<PlayerRecord>> GetPlayerResult() => new(new Return<PlayerRecord>(new PlayerRecord(6, 1)));
        public WaitResult<bool> SetScore(long score) => new WaitResultSource<bool>(true);
        public WaitResultSource<Return<Leaderboard>> GetLeaderboard(int quantityTop, bool includeUser = false, int quantityAround = 0, AvatarSize size = AvatarSize.Small)
        {
            Debug.Log(_lbName);

            List<LeaderboardRecord> list = new()
        {
            new(1, 1100, "����� ������", ""),
            new(2, 1000, "�������� �������", "https://pixelbox.ru/wp-content/uploads/2021/10/dark-avatar-vk-pixelbox.ru-87.jpg"),
            new(3, 900, "������ ������", "������ ������"),
            new(4, 800, "����� Ը���", ""),
            new(5, 600, "������ ����", ""),
            new(6, 550, "�������� ����", ""),
            new(8, 500, "", ""),
            new(9, 400, "�������� ����", ""),
            new(10, 300, "�������� �������", "https://pixelbox.ru/wp-content/uploads/2021/10/dark-avatar-vk-pixelbox.ru-7-150x150.png"),
            new(11, 200, "������� �����", ""),
            new(12, 100, "������� ����", "")
        };

            Leaderboard l = new(2, list.ToArray());

            return new(new Return<Leaderboard>(l));
        }

        public WaitResultSource<Return<Leaderboard>> GetLeaderboardTest()
        {
            List<LeaderboardRecord> list = new()
        {
            new(1, 1100, "����� ������", ""),
            new(2, 1000, "�������� �������", ""),
            new(3, 900, "������ ������", ""),
            new(4, 800, "����� Ը���", ""),
            new(5, 600, "������ ����", ""),
            new(6, 550, "�������� ����", ""),
            new(7, 500, "", ""),
            new(9, 400, "�������� ����", ""),
            new(10, 300, "�������� �������", ""),
        };

            Leaderboard l = new(2, list.ToArray());

            return new(new Return<Leaderboard>(l));
        }

        public WaitResultSource<bool> Save(string key, string data)
        {
            using StreamWriter sw = new(Path.Combine(Application.persistentDataPath, key));
            sw.Write(data);

            return new(true);
        }
        public WaitResultSource<string> Load(string key)
        {
            string path = Path.Combine(Application.persistentDataPath, key);
            if (File.Exists(path))
            {
                using StreamReader sr = new(path);
                return new(sr.ReadToEnd());
            }
            return new(string.Empty);
        }

        public WaitResultSource<bool> CanReview() => new(IsLogOn);
        public WaitResultSource<bool> RequestReview() => new(true);

        public WaitResultSource<bool> CanShortcut() => new(IsLogOn);
        public WaitResultSource<bool> CreateShortcut() => new(IsLogOn);

    }
}
#endif
