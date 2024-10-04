namespace Vurbiri.Colonization
{
    public class SignpostGraphic : AEdificeSidesGraphic<SignpostSide>
    {
        private Players _players;

        public override void Initialize(PlayerType owner, IdHashSet<LinkId, CrossroadLink> links)
        {
            _players = Players.Instance;

            foreach (var side in _graphicSides)
                side.SetActive(links.ContainsKey(side.Id));
        }

        public override void AddRoad(Id<LinkId> linkId, PlayerType owner) => _graphicSides[linkId].AddRoad(_players[owner].MaterialUnlit);
    }
}
