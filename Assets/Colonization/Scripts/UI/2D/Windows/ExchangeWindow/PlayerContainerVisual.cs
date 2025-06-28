using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
	public class PlayerContainerVisual : MonoBehaviour
	{
		[SerializeField, Range(0f, 1f)] private float _borderAlpha = 0.5f;
		[Space]
		[SerializeField] private Image _border;
		[SerializeField] private TextMeshProUGUI _caption;
		
		public void Init()
		{
			var color = SceneContainer.Get<PlayerColors>()[PlayerId.Player];
            _caption.text = SceneContainer.Get<PlayerNames>()[PlayerId.Player];
            _caption.color = color;

            color.a = _borderAlpha;
            _border.color = color;

			Destroy(this);
        }
		
#if UNITY_EDITOR
        private void OnValidate()
        {
			this.SetComponent(ref _border);
			this.SetChildren(ref _caption, "Caption");
        }
#endif
	}
}
