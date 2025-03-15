//Assets\Colonization\Scripts\Diplomacy\DiplomacySettings.cs
using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable]
	public class DiplomacySettings
	{
		[Range( 10, 50)] public int defaultValue = 25;
        [Range( -5, -1)] public int penaltyPerRound = -1;
        [Range(1, 10)] public int rewardForBuff = 10;
        [Range(-10, -1)] public int penaltyForFireOnEnemy = -5;
        [Range(-15, -1)] public int penaltyForFriendlyFire = -10;
    }
}
