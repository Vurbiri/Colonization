using System.Collections;

namespace Vurbiri.Colonization.EntryPoint
{
	sealed internal class CreatePlayers : ALocalizationLoadingStep
	{
		private readonly GameContent _content;
		private readonly Player.Settings _playerSettings;

		public CreatePlayers(GameContent content, Player.Settings playerSettings) : base("PlayersCreationStep")
		{
			_content = content;
			_playerSettings = playerSettings;
		}

		public override IEnumerator GetEnumerator()
		{
			WaitAllWaits waitSpawn = new();
			_content.players = new Players(_playerSettings, _content.gameLoop, waitSpawn);
			yield return null;
			SpellBook.Init();

			yield return new WaitRealtime(.33f);
			yield return waitSpawn;
		}
	}
}
