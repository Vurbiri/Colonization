using UnityEngine;

namespace Vurbiri.Colonization
{
    public class UrbanGraphic : AEdificeGraphic
    {
        public override void Init(Id<PlayerId> playerId, IdHashSet<LinkId, CrossroadLink> links)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(SceneObjects.Get<Players>()[playerId].MaterialLit, _idMaterial);

            foreach (var link in links)
            {
                if (link.Owner == playerId)
                {
                    transform.localRotation = CONST.LINK_ROTATIONS[link.Id];
                    return;
                }
            }
        }
    }
}
