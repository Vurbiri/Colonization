using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class WarriorsAISettings : ActorsAISettings<WarriorId, WarriorAIStateId>
    {
        [Range(0, 100)] public int ratioForEscape;
        [Range(0, 100)] public int ratioForAttack;
        [Space]
        [Range(0, 5)] public int maxDistanceHelp;
        [Range(0, 100)] public int minHPHelp;
        [Space]
        [Range(0, 5)]   public int maxDistanceUnsiege;
        [Range(0, 100)] public int minHPUnsiege;
        [Space]
        [Range(0, 5)] public int maxDistanceRaid;
        [Range(0, 100)] public int minHPRaid;
        [Space]
        [Range(0, 5)] public int maxDistanceHome;
    }
}
