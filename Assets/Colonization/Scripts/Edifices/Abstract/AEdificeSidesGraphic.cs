using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class AEdificeSidesGraphic : AEdificeGraphic
    {
        [Space]
        [SerializeField] protected EnumHashSet<LinkType, AEdificeSide> _graphicSides;

        protected Players _players;

        public override void AddRoad(LinkType type, PlayerType owner) => _graphicSides[type].AddRoad(_players[owner].Material);
    }
}
