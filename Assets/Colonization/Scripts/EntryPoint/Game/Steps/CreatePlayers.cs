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
            _content.players = new Players(_playerSettings, _content);
            yield return null;
        }
    }
}
