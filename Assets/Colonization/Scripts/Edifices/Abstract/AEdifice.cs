//Assets\Colonization\Scripts\Edifices\Abstract\AEdifice.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class AEdifice : MonoBehaviour, IValueId<EdificeId>, ISelectableReference
    {
        [SerializeField] protected EdificeSettings _settings;
        [Space]
        [SerializeField] protected AEdificeGraphic _graphic;

        public Id<EdificeId> Id => _settings.id;
        public EdificeSettings Settings => _settings;
        public virtual bool RaycastTarget { get => false; set { } }
        public ISelectable Selectable { get; set; }

        public virtual AEdifice Init(Id<PlayerId> playerId, bool isWall, IReadOnlyList<CrossroadLink> links, AEdifice edifice)
        {
            Selectable = edifice.Selectable;
            transform.SetParent(edifice.transform.parent);
            transform.SetLocalPositionAndRotation(edifice.transform);

            if(edifice._graphic != null)
                _graphic.transform.localRotation = edifice._graphic.transform.localRotation;
            _graphic.Init(playerId, links);

            Destroy(edifice.gameObject);
            return this;
        }

        public virtual bool WallBuild(Id<PlayerId> owner, IReadOnlyList<CrossroadLink> links) => false;
        public virtual void AddRoad(Id<LinkId> linkId, bool isWall) { }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            _settings.groupId = _settings.id.ToGroup();
            _settings.nextGroupId = _settings.nextId.ToGroup();

            _settings.isBuildWall = _settings.groupId == EdificeGroupId.Urban && _settings.id != EdificeId.Camp;

            _settings.isUpgrade = _settings.groupId == EdificeGroupId.None || _settings.id == EdificeId.PortOne || _settings.id == EdificeId.PortTwo
                || (_settings.groupId == EdificeGroupId.Urban && _settings.id != EdificeId.City);

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
