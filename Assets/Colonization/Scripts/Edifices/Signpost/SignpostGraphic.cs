namespace Vurbiri.Colonization
{
    public class SignpostGraphic : AEdificeSidesGraphic<SignpostSide>
    {
        private Players _players;

        public override void Initialize(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            _players = Players.Instance;

            foreach (var side in _graphicSides)
                side.SetActive(links.ContainsKey(side.Type));
        }

        public override void AddRoad(LinkType type, PlayerType owner) => _graphicSides[type].AddRoad(_players[owner].MaterialUnlit);
    }
}
