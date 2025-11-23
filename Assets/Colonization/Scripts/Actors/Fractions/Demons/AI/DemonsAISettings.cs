using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class DemonsAISettings : ActorsAISettings<DemonId, DemonAIStateId>
    {
        [Header("MoveToEnemy")]
        [Range(1, 5)] public int maxDistanceToEnemy;
        [Range(0f, 3f)] public float enemyForceRatio;
    }
}
