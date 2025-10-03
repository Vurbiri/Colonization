using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
	[System.Serializable]
	public class HexWeight
	{
		[UnityEngine.SerializeField, Newtonsoft.Json.JsonProperty] 
		private Array<int> _hexWeight;

		public static implicit operator ReadOnlyArray<int>(HexWeight self) => self._hexWeight;

#if UNITY_EDITOR
		public void OnValidate() => EUtility.SetArray(ref _hexWeight, CONST.DICE_MAX * 2 + 1);
#endif
    }
}
