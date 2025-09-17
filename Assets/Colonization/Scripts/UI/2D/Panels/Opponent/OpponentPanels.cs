using UnityEngine;

namespace Vurbiri.Colonization.UI
{
	public class OpponentPanels : MonoBehaviour
	{
		[SerializeField] private OpponentPanel[] _panels;
		
		public void Init()
		{
            for (int i = 0; i < PlayerId.AICount; i++)
				_panels[i].Init();
		}
		
#if UNITY_EDITOR
        private void OnValidate()
        {
			this.SetChildrens(ref _panels, PlayerId.AICount);
        }
#endif
	}
}
