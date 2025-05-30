using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public abstract class AEdificeGraphic : MonoBehaviour
    {
        [SerializeField] protected EdificeSFX _edificeSFX;
        [Space]
        [SerializeField] protected MeshRenderer _meshRenderer;
        [SerializeField, Range(0, 5)] protected int _idMaterial;

        public virtual WaitSignal Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links, bool isSFX)
        {
            _meshRenderer.SetSharedMaterial(SceneContainer.Get<HumansMaterials>()[playerId].materialLit, _idMaterial);

            Destroy(this);
            return isSFX ? _edificeSFX.Run(transform) : _edificeSFX.Destroy();
        }

        public virtual void AddRoad(Id<LinkId> linkId) { }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();
            if(_edificeSFX == null)
                _edificeSFX = transform.parent.GetComponentInChildren<EdificeSFX>();
        }
#endif

    }
}
