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
        [SerializeField] private ReadOnlyIdSet<LinkId, WallGate> _graphicSides;

        public WaitSignal Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links, bool isSFX)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(GameContainer.Materials[playerId].Lit, _idMaterial);

            foreach (var link in links)
                _graphicSides[link.Id].Open(link.IsOwner);

            gameObject.SetActive(true);

            var wallSFX = GetComponentInChildren<WallSFX>();
            return isSFX ? wallSFX.Run(transform) : wallSFX.Destroy();
        }

        public void AddRoad(Id<LinkId> linkId) => _graphicSides[linkId].Open(true);
        public void RemoveRoad(Id<LinkId> linkId) => _graphicSides[linkId].Open(false);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying) return;

            if (_graphicSides == null || _graphicSides.IsNotFull)
                _graphicSides = new(GetComponentsInChildren<WallGate>());
        }
#endif
    }
}
