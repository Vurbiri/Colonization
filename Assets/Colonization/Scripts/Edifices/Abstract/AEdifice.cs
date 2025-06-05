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

        protected Transform _graphicTransform;

        public Id<EdificeId> Id => _settings.id;
        public EdificeSettings Settings => _settings;
        public ISelectable Selectable { get; set; }

        public virtual WaitSignal Init(Id<PlayerId> playerId, bool isWall, IReadOnlyList<CrossroadLink> links, AEdifice oldEdifice, bool isSFX)
        {
            _graphicTransform = _graphic.transform;
            Transform thisTransform = transform, oldTransform = oldEdifice.transform;
            
            Selectable = oldEdifice.Selectable;
            thisTransform.SetParent(oldTransform.parent);
            thisTransform.SetLocalPositionAndRotation(oldTransform.localPosition, oldTransform.localRotation);

            if (oldEdifice._graphicTransform != null)
                _graphicTransform.localRotation = oldEdifice._graphicTransform.localRotation;

            var wait = _graphic.Init(playerId, links, isSFX);

            _graphic = null;
            Destroy(oldEdifice.gameObject);

            return wait;
        }

        public virtual ReturnSignal WallBuild(Id<PlayerId> owner, IReadOnlyList<CrossroadLink> links, bool isSFX) => false;
        public virtual Wall WallTransfer(Transform newParent) => null;
        public virtual void AddRoad(Id<LinkId> linkId, bool isWall) { }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (Application.isPlaying) return;
                        
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
        }
#endif
    }
}
