using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    sealed public class Wall : MonoBehaviour
    {
        [SerializeField] private WallSFX _wallSFX;
        [Space]
        [SerializeField, Range(0, 5)] private int _idMaterial;
        [Space]
        [SerializeField] private IdSet<LinkId, WallGate> _graphicSides;

        public WaitSignal Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links, bool isSFX)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(SceneContainer.Get<HumansMaterials>()[playerId].materialLit, _idMaterial);

            foreach (var link in links)
                _graphicSides[link.Id].Open(link.Owner != PlayerId.None);

            gameObject.SetActive(true);

            return isSFX ? _wallSFX.Run(transform) : _wallSFX.Destroy();
        }

        public void AddRoad(Id<LinkId> linkId) => _graphicSides[linkId].Open(true);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_wallSFX == null)
                _wallSFX = GetComponentInChildren<WallSFX>();
            if (_graphicSides.Fullness < _graphicSides.Count)
                _graphicSides.ReplaceRange(GetComponentsInChildren<WallGate>());
        }
#endif
    }
}
