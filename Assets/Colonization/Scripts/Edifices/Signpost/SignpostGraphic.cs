namespace Vurbiri.Colonization
{
    public class SignpostGraphic : AEdificeSidesGraphic
    {
        public override void Initialize(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            _players = Players.Instance;

            foreach (var side in _graphicSides)
                side.SetActive(links.ContainsKey(side.Type));
        }
    }
}
