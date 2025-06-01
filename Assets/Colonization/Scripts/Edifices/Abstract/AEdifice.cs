using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(Collider))]
    public abstract class AEdifice : MonoBehaviour, IValueId<EdificeId>, ISelectableReference
    {
        [SerializeField] protected EdificeSettings _settings;
        [Space]
        [SerializeField] protected AEdificeGraphic _graphic;
        [SerializeField] protected SphereCollider _thisCollider;

        public Id<EdificeId> Id => _settings.id;
        public EdificeSettings Settings => _settings;
        public ISelectable Selectable { get; set; }
        public bool RaycastTarget { get => _thisCollider.enabled; set => _thisCollider.enabled = value; }

        public virtual WaitSignal Init(Id<PlayerId> playerId, bool isWall, IReadOnlyList<CrossroadLink> links, AEdifice oldEdifice, bool isSFX)
        {
            Destroy(oldEdifice.gameObject);

            Transform thisTransform = transform, oldTransform = oldEdifice.transform;

            Selectable = oldEdifice.Selectable;
            thisTransform.SetParent(oldTransform.parent);
            thisTransform.SetLocalPositionAndRotation(oldTransform.localPosition, oldTransform.localRotation);

            if (oldEdifice._graphic != null)
                _graphic.transform.localRotation = oldEdifice._graphic.transform.localRotation;

            return _graphic.Init(playerId, links, isSFX);
        }

        public virtual ReturnSignal WallBuild(Id<PlayerId> owner, IReadOnlyList<CrossroadLink> links, bool isSFX) => false;
        public virtual Wall WallTransfer(Transform newParent) => null;
        public virtual void AddRoad(Id<LinkId> linkId, bool isWall) { }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            _settings.groupId = _settings.id.ToGroup();
            _settings.nextGroupId = _settings.nextId.ToGroup();

            _settings.isBuildWall = _settings.groupId == EdificeGroupId.Colony && _settings.id != EdificeId.Camp;

            _settings.isUpgrade = _settings.groupId == EdificeGroupId.None || _settings.id == EdificeId.PortOne || _settings.id == EdificeId.PortTwo
                || (_settings.groupId == EdificeGroupId.Colony && _settings.id != EdificeId.City);

            _settings.profit = 0;
            if (_settings.id == EdificeId.Camp || _settings.id == EdificeId.PortOne || _settings.id == EdificeId.PortTwo)
                _settings.profit = 1;
            else if (_settings.id == EdificeId.Town || _settings.groupId == EdificeGroupId.Port)
                _settings.profit = 2;
            else if (_settings.id == EdificeId.City)
                _settings.profit = 3;
            else
                _settings.profit = 0;

            if (_graphic == null)
                _graphic = GetComponentInChildren<AEdificeGraphic>();

            if (_thisCollider == null)
                _thisCollider = GetComponent<SphereCollider>();
        }
#endif
    }
}
