using UnityEngine;

namespace Vurbiri.Colonization.UI
{
	public class OpponentPanels : MonoBehaviour
	{
        [SerializeField] private Direction2 _directionPopup;
		[Space]
        [SerializeField] private OpponentPanel[] _panels;
		
		public void Init()
		{
            Vector3 direction = _directionPopup;
			Diplomacy diplomacy = GameContainer.Diplomacy;

            for (int i = 0; i < PlayerId.AICount; i++)
				_panels[i].Init(direction, diplomacy);
		}
		
#if UNITY_EDITOR
        private void OnValidate()
        {
			this.SetChildrens(ref _panels, PlayerId.AICount);
        }
#endif
	}
}
