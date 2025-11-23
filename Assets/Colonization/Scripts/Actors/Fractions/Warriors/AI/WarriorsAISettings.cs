using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class WarriorsAISettings : ActorsAISettings<WarriorId, WarriorAIStateId>
    {
        [Header("Escape")]
        [Range(0, 100)] public int ratioForEscape;
        [Header("BlockInCombat")]
        [Range(0, 100)] public int ratioForBlock;
        [Range(0, 100)] public int maxHPForBlock;
        [Header("MoveToUnsiege")]
        [Range(1, 5)]   public int maxDistanceUnsiege;
        [Range(0, 100)] public int minHPUnsiege;
        [Header("MoveToHome")]
        [Range(1, 5)] public int maxDistanceHome;
        [Header("FindResources")]
        public Chance chanceFreeFinding;
    }
}
