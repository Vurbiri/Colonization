#if UNITY_EDITOR

using NaughtyAttributes;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class YandexSDK
{
    [Space]
    [SerializeField] private string _playerName = "Best of the Best";
    [SerializeField] private bool _isDesktop = true;
    [SerializeField] private bool _isLogOn = true;
    [SerializeField, Dropdown("GetLangValues")] private string _lang = "ru";
    [SerializeField] private bool _isInitialize = true;
    [SerializeField] private bool _isLeaderboard = true;
    [SerializeField] private bool _isPlayer = true;

    private DropdownList<string> GetLangValues()
    {
        return new DropdownList<string>()
        {
            { "Русский",  "ru" },
            { "English",  "en" }
        };
    }

    public bool IsDesktop => _isDesktop;

    public bool IsInitialize => _isInitialize;
    public bool IsPlayer => IsInitialize && _isPlayer;
    public bool IsLeaderboard => IsLogOn && _isLeaderboard;
    public string PlayerName => _playerName;
    public bool IsLogOn => _isLogOn;
    public string Lang => _lang;

    public WaitResult<bool> InitYsdk() => new(_isInitialize);
    public void LoadingAPI_Ready() { }
    public WaitResult<bool> InitPlayer() => new(_isPlayer);
    public WaitResult<bool> LogOn()
    {
        _isLogOn = true;
        return new(_isLogOn); ;
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
            new(1, 1100, "Седов Герман", ""),
            new(2, 1000, "Журавлев Тимофей", "https://pixelbox.ru/wp-content/uploads/2021/10/dark-avatar-vk-pixelbox.ru-87.jpg"),
            new(3, 900, "Крылов Богдан", "Крылов Богдан"),
            new(4, 800, "Панов Фёдор", ""),
            new(5, 600, "Зайцев Илья", ""),
            new(6, 550, "Лебедева Алёна", ""),
            new(8, 500, "", ""),
            new(9, 400, "Муравьев Егор", ""),
            new(10, 300, "Казанцев Алексей", "https://pixelbox.ru/wp-content/uploads/2021/10/dark-avatar-vk-pixelbox.ru-7-150x150.png"),
            new(11, 200, "Баженов Борис", ""),
            new(12, 100, "Крылова Таня", "")
        };

        Leaderboard l = new(2, list.ToArray());

        return new(new Return<Leaderboard>(l));
    }

    public WaitResult<Return<Leaderboard>> GetLeaderboardTest()
    {
        List<LeaderboardRecord> list = new()
        {
            new(1, 1100, "Седов Герман", ""),
            new(2, 1000, "Журавлев Тимофей", ""),
            new(3, 900, "Крылов Богдан", ""),
            new(4, 800, "Панов Фёдор", ""),
            new(5, 600, "Зайцев Илья", ""),
            new(6, 550, "Лебедева Алёна", ""),
            new(7, 500, "", ""),
            new(9, 400, "Муравьев Егор", ""),
            new(10, 300, "Казанцев Алексей", ""),
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

    public WaitResult<bool> CanReview() => new(_isLogOn);
    public WaitResult<bool> RequestReview() => new(true); 

    public WaitResult<bool> CanShortcut() => new(_isLogOn);
    public WaitResult<bool> CreateShortcut() => new(_isLogOn);

}
#endif
