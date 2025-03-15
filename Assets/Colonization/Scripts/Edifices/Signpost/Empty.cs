//Assets\Colonization\Scripts\Edifices\Signpost\Signpost.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    sealed public class Empty : AEdificeSelectable
    {
        public override AEdifice Init(Id<PlayerId> playerId, bool isWall, IReadOnlyList<CrossroadLink> links, AEdifice edifice)
        {
            return this;
        }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            _settings.id = EdificeId.Empty;
            _settings.groupId = EdificeGroupId.None;
            _settings.nextId = EdificeId.Empty;
            _settings.nextGroupId = EdificeGroupId.None;

            _settings.isBuildWall = false;
            _settings.isUpgrade = true;

            _settings.profit = 0;

            if (_collider == null)
                _collider = GetComponent<Collider>();
        }
#endif
    }
}
