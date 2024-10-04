using UnityEngine;

namespace Vurbiri.Colonization
{
    public class UrbanGraphic : AEdificeGraphic
    {
        public override void Initialize(PlayerType owner, IdHashSet<LinkId, CrossroadLink> links)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(Players.Instance[owner].MaterialLit, _idMaterial);

            foreach (var link in links)
            {
                if (link.Owner == owner)
                {
                    transform.localRotation = CONST.LINK_ROTATIONS[link.Id];
                    return;
                }
            }
        }
    }
}
