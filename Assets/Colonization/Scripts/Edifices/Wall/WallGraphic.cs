using UnityEngine;

namespace Vurbiri.Colonization
{
    public class WallGraphic : AEdificeSidesGraphic
    {
        public override void Initialize(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            _players = Players.Instance;

            GetComponent<MeshRenderer>().SetSharedMaterial(_players[owner].Material, _idMaterial);

            LinkType type;
            foreach (var link in links)
            {
                type = link.Type;
                owner = link.Owner;

                if(owner == PlayerType.None)
                    _graphicSides[type].SetActive(false);
                else
                    _graphicSides[type].AddRoad(_players[owner].Material);
            }
   
        }
    }
}
