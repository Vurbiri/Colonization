//Assets\Colonization\Scripts\UI\_UIGame\Panels\Currencies\BloodPanel.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    public partial class BloodPanel : MonoBehaviour
	{
        [SerializeField] private Blood _blood;
        [Space]
        [SerializeField] private float _transparency = 0.84f;
        [Space]
        [SerializeField] private Direction2 _directionPopup;

        private void Start()
        {
            SceneContainer.Get<GameplayEventBus>().EventSceneEndCreation += Create;
        }

        private void Create()
        {
            GetComponent<Image>().color = SceneContainer.Get<PlayersVisual>()[PlayerId.Player].color.SetAlpha(_transparency);

            var currencies = SceneContainer.Get<Players>().Player.Resources;

            _blood.Init(currencies.BloodCurrent, currencies.BloodMax, SceneContainer.Get<Vurbiri.UI.TextColorSettings>(), _directionPopup);

            SceneContainer.Get<GameplayEventBus>().EventSceneEndCreation -= Create;
            Destroy(this);
        }
	}
}
