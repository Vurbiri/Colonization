//Assets\Colonization\Scripts\Diplomacy\DiplomacySettings.cs
using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable]
	public class DiplomacySettings
	{
		[Range( 10, 50)] public int defaultValue;
        [Range( -5, -1)] public int penaltyPerRound;
        [Range(1, 10)] public int rewardForBuff;
        [Range(-10, -1)] public int penaltyForAttackingEnemy;
        [Range(-15, -1)] public int penaltyForFriendlyFire;

    }
}
