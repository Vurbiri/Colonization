using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(Collider))]
    public abstract class AEdifice : MonoBehaviour, IValueId<EdificeId>, ISelectableReference
    {
        [SerializeField] protected EdificeSettings _settings;
        [Space]
        [SerializeField] protected AEdificeGraphic _graphic;

        protected Transform _graphicTransform;
        private Key _key;

        public Id<EdificeId> Id { [Impl(256)] get => _settings.id; }
        public EdificeSettings Settings { [Impl(256)] get => _settings; }
        public ISelectable Selectable { [Impl(256)] get => GameContainer.Crossroads[_key]; }
        public Key Key { [Impl(256)] set => _key = value; }

        public virtual WaitSignal Init(Id<PlayerId> playerId, bool isWall, IReadOnlyList<CrossroadLink> links, AEdifice oldEdifice, bool isSFX)
        {
            _graphicTransform = _graphic.GetComponent<Transform>();
            Transform thisTransform = GetComponent<Transform>(), oldTransform = oldEdifice.GetComponent<Transform>();

            _key = oldEdifice._key;
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
        public virtual void RemoveRoad(Id<LinkId> linkId, bool isWall) { }

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
            _settings.wallDefense = 0;
            if (_settings.id == EdificeId.Camp || _settings.id == EdificeId.PortOne || _settings.id == EdificeId.PortTwo)
            {
                _settings.profit = 1;
            }
            else if (_settings.id == EdificeId.Town || _settings.groupId == EdificeGroupId.Port)
            {
                _settings.profit = 2;
            }
            else if (_settings.id == EdificeId.City)
            {
                _settings.profit = 3;
                _settings.wallDefense = 2;
            }
            else
            {
                _settings.profit = 0;
            }

            if (_settings.id == EdificeId.Town)
            {
                _settings.wallDefense = 1;
            }

            if (_graphic == null)
                _graphic = GetComponentInChildren<AEdificeGraphic>();
        }
#endif
    }
}
