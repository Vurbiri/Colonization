//Assets\Colonization\Scripts\Diplomacy\DiplomacySettings.cs
using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable]
	public class DiplomacySettings
	{
		[Range(0, 50)] public int defaultValue;
        [Range(0, 50)] public int penalty;
    }
}
