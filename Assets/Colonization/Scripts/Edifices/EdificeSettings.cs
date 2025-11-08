using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class EdificeSettings
    {
        public Id<EdificeId> id;
        [ReadOnly] public Id<EdificeGroupId> groupId;
        [Space]
        [ReadOnly] public Id<EdificeId> nextId;
        [ReadOnly] public Id<EdificeGroupId> nextGroupId;
        [Space]
        [ReadOnly] public int profit;
        [Space]
        [ReadOnly] public bool isUpgrade;
        [ReadOnly] public bool isBuildWall;
        [ReadOnly] public int wallDefense;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNextId(Id<EdificeId> id, Id<EdificeGroupId> groupId)
        {
            nextId = id;
            nextGroupId = groupId;
        }
    }
}
