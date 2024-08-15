using UnityEngine;

namespace Vurbiri.Colonization
{
    public class UrbanGraphic : AEdificeGraphic
    {
        public bool IsPositioning { get; set; }

        public override void Initialize(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(Players.Instance[owner].MaterialLit, _idMaterial);

            if (!IsPositioning)
                return;

            foreach (var link in links)
            {
                if (link.Owner == owner)
                {
                    transform.localRotation = CONST.LINK_ROTATIONS[link.Type];
                    return;
                }
            }
        }
    }
}
