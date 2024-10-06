namespace Vurbiri.Colonization
{
    public class SignpostGraphic : AEdificeSidesGraphic<SignpostSide>
    {
        private Players _players;

        public override void Initialize(Id<PlayerId> playerId, IdHashSet<LinkId, CrossroadLink> links)
        {
            _players = Players.Instance;

            foreach (var side in _graphicSides)
                side.SetActive(links.ContainsKey(side.Id));
        }

        public override void AddRoad(Id<LinkId> linkId, Id<PlayerId> playerId) => _graphicSides[linkId].AddRoad(_players[playerId].MaterialUnlit);
    }
}
