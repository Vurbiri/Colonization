using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    sealed public class Wall : MonoBehaviour
    {
        [SerializeField, Range(0, 5)] private int _idMaterial;
        [Space]
        [SerializeField] private IdSet<LinkId, WallGate> _graphicSides;

        public Wall Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(SceneContainer.Get<HumansMaterials>()[playerId].materialLit, _idMaterial);

            foreach (var link in links)
                _graphicSides[link.Id].Open(link.Owner != PlayerId.None);

            gameObject.SetActive(true);

            return this;
        }

        public void AddRoad(Id<LinkId> linkId) => _graphicSides[linkId].Open(true);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_graphicSides.Fullness < _graphicSides.Count)
                _graphicSides.ReplaceRange(GetComponentsInChildren<WallGate>());
        }
#endif
    }
}
