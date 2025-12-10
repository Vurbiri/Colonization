using UnityEngine;

namespace Vurbiri.Colonization
{
	public class Tutorial : MonoBehaviour
	{
		private void Awake()
		{
			
		}

		public enum State
		{
			None = -1,
			Start = 0,
		}
		
#if UNITY_EDITOR
        private void OnValidate()
        {
			
        }
#endif
	}
}
