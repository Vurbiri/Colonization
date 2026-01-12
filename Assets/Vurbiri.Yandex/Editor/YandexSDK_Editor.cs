#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Vurbiri.Yandex
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

		public WaitResult<bool> InitYsdk() => WaitResult.Instant(_isInitialize);
		public void LoadingAPI_Ready() { }
		public WaitResult<bool> InitPlayer() => WaitResult.Instant(_isPlayer);
		public WaitResult<bool> LogOn()
		{
			IsLogOn = true;
			return WaitResult.Instant(IsLogOn);
		}
		public WaitResult<bool> InitLeaderboards() => WaitResult.Instant(IsLeaderboard);
		public string GetPlayerAvatarURL(AvatarSize size) => string.Empty;

		public WaitResult<Return<PlayerRecord>> GetPlayerResult() => WaitResult.Instant(new Return<PlayerRecord>(new PlayerRecord(6, 1)));
		public WaitResult<bool> SetScore(long score) => WaitResult.Instant(true);
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

			return WaitResult.Instant(new Return<Leaderboard>(l));
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
                new(8, 500, "", ""),
                new(9, 400, "Муравьев Егор", ""),
                new(10, 300, "Казанцев Алексей", ""),
                new(11, 200, "Баженов Борис", ""),
                new(12, 100, "Крылова Таня", "")
            };

			Leaderboard l = new(2, list.ToArray());

			return WaitResult.Instant(new Return<Leaderboard>(l));
		}

		public IEnumerator Save(string key, string data, WaitResultSource<bool> waitResult)
		{
			using StreamWriter sw = new(Path.Combine(Application.persistentDataPath, key));
			sw.Write(data);
			return waitResult.Return(true);
		}
		public WaitResult<string> Load(string key)
		{
			string path = Path.Combine(Application.persistentDataPath, key), result = string.Empty;
			if (File.Exists(path))
			{
				using StreamReader sr = new(path);
				result = sr.ReadToEnd();
			}
			return WaitResult.Instant(result);
		}

		public WaitResult<bool> CanReview() => WaitResult.Instant(IsLogOn);
		public WaitResult<bool> RequestReview() => WaitResult.Instant(true);

		public WaitResult<bool> CanShortcut() => WaitResult.Instant(IsLogOn);
		public WaitResult<bool> CreateShortcut() => WaitResult.Instant(IsLogOn);

	}
}
#endif
