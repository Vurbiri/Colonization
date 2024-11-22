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

        public WaitResult<bool> InitYsdk() => new(_isInitialize);
        public void LoadingAPI_Ready() { }
        public WaitResult<bool> InitPlayer() => new(_isPlayer);
        public WaitResult<bool> LogOn()
        {
            IsLogOn = true;
            return new(IsLogOn); ;
        }
        public WaitResult<bool> InitLeaderboards() => new(IsLeaderboard);
        public string GetPlayerAvatarURL(AvatarSize size) => string.Empty;

        public WaitResult<Return<PlayerRecord>> GetPlayerResult() => new(new Return<PlayerRecord>(new PlayerRecord(6, 1)));
        public WaitResult<bool> SetScore(long score) => new(true);
        public WaitResult<Return<Leaderboard>> GetLeaderboard(int quantityTop, bool includeUser = false, int quantityAround = 0, AvatarSize size = AvatarSize.Small)
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

        public WaitResult<Return<Leaderboard>> GetLeaderboardTest()
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

        public WaitResult<bool> Save(string key, string data)
        {
            using StreamWriter sw = new(Path.Combine(Application.persistentDataPath, key));
            sw.Write(data);

            return new(true);
        }
        public WaitResult<string> Load(string key)
        {
            string path = Path.Combine(Application.persistentDataPath, key);
            if (File.Exists(path))
            {
                using StreamReader sr = new(path);
                return new(sr.ReadToEnd());
            }
            return new(string.Empty);
        }

        public WaitResult<bool> CanReview() => new(IsLogOn);
        public WaitResult<bool> RequestReview() => new(true);

        public WaitResult<bool> CanShortcut() => new(IsLogOn);
        public WaitResult<bool> CreateShortcut() => new(IsLogOn);

    }
}
#endif
