//Assets\Colonization\Scripts\Edifices\EdificeSettings.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class EdificeSettings
    {
        public Id<EdificeId> id;
        public Id<EdificeGroupId> groupId;
        [Space]
        public Id<EdificeId> nextId;
        public Id<EdificeGroupId> nextGroupId;
        [Space]
        [Range(0, 3)] public int profit;
        [Space]
        public bool isUpgrade;
        public bool isBuildWall;

        public void SetNextId(Id<EdificeId> id)
        {
            nextId = id;
            nextGroupId = id.ToGroup();
        }
    }
}
