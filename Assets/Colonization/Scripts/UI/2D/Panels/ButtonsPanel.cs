using UnityEngine;

namespace Vurbiri.Colonization.UI
{
	public class ButtonsPanel : MonoBehaviour
	{
        [SerializeField] private HintToggle _hexagonCaption;
        [SerializeField] private HintToggle _trackingCamera;

        public void Init()
		{
            _hexagonCaption.Init(GameContainer.GameSettings.HexagonShow);
            _trackingCamera.Init(GameContainer.GameSettings.TrackingCamera);

            Destroy(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                this.SetChildren(ref _hexagonCaption, "HexagonCaption");
                this.SetChildren(ref _trackingCamera, "TrackingCamera");
            }
        }
#endif
    }
}
