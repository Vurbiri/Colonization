using UnityEngine;

namespace Vurbiri.Colonization
{
	[System.Serializable]
	public class AIControllerSettings
	{
		[MinMax(1, 8)] public WaitRealtime waitPlayStart; // = 3
		[MinMax(1, 8)] public WaitRealtime waitPlay; // = 4
		[Space]
		public Id<PlayerId> militarist; // = 1
        [Space]
        public Exchange exchange;
        [Space]
		public Cheat cheat;

		#region Nested : Exchange, Cheat, Cheat.FreePerks
		//************************************************************
		[System.Serializable]
		public struct Exchange
		{
            [MinMax(5, 30)] public int percentBloodOffset; // = 10
            [MinMax(1, 8)] public int minExchangeBlood; // = 2
            [MinMax(50, 100)] public int minPercentAmount; // = 10
        }
		//************************************************************
		[System.Serializable]
		public struct Cheat
		{
			[MinMax(0, 50)] public int minPercentRes; //= 25;
			[MinMax(0, 5)] public int addRes; //= 2;
			[Space]
			public FreePerks freePerks;

			public readonly void TryAddRes(Currencies resources)
			{
				if (resources.PercentAmount < minPercentRes)
					resources.AddToMin(addRes);
			}

			//-----------------------------------------------
			[System.Serializable]
			public struct FreePerks
			{
				[MinMax(0, 50)] public int turn; // = 15;
				public Id<EconomicPerksId> economic;
				public Id<MilitaryPerksId> military;
			}
		}
		//-----------------------------------------------
		#endregion
	}
}
