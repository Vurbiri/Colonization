//Assets\Colonization\Scripts\Edifices\Wall\Wall.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Wall : MonoBehaviour
    {
        [SerializeField, Range(0, 5)] protected int _idMaterial;
        [Space]
        [SerializeField] protected IdHashSet<LinkId, WallGate> _graphicSides;

        public Wall Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(SceneData.Get<PlayersVisual>()[playerId].materialLit, _idMaterial);

            foreach (var link in links)
                _graphicSides[link.Id].Open(link.Owner != PlayerId.None);

            gameObject.SetActive(true);

            return this;
        }

        public void AddRoad(Id<LinkId> linkId) => _graphicSides[linkId].Open(true);
    }
}
