//Assets\Colonization\Scripts\Edifices\Abstract\AEdificeGraphicReColor.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class AEdificeGraphicReColor : AEdificeGraphic
    {
        [SerializeField, Range(0, 5)] protected int _idMaterial;

        public override void Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(SceneContainer.Get<PlayersVisual>()[playerId].materialLit, _idMaterial);
        }
    }
}
