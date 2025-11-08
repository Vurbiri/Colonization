using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    sealed public class Empty : AEdifice
    {
        public override WaitSignal Init(Id<PlayerId> playerId, bool isWall, IReadOnlyList<CrossroadLink> links, AEdifice edifice, bool isSFX)
        {
            return null;
        }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            _settings.id = EdificeId.Empty;
            _settings.groupId = EdificeGroupId.None;
            _settings.nextId = EdificeId.None;
            _settings.nextGroupId = EdificeGroupId.None;

            _settings.isBuildWall = false;
            _settings.isUpgrade = true;

            _settings.profit = 0;
        }
#endif
    }
}
