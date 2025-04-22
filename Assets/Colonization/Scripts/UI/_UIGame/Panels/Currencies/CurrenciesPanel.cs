//Assets\Colonization\Scripts\UI\_UIGame\Panels\Currencies\CurrenciesPanel.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    public partial class CurrenciesPanel : MonoBehaviour
    {
        [SerializeField] private Currency[] _currencies;
        [SerializeField] private Amount _amount;
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
            var settings = SceneContainer.Get<Vurbiri.UI.TextColorSettings>();

            for (int i = 0; i < CurrencyId.CountMain; i++)
                _currencies[i].Init(i, currencies, settings, _directionPopup);

            _amount.Init(currencies.AmountCurrent, currencies.AmountMax, settings);

            SceneContainer.Get<GameplayEventBus>().EventSceneEndCreation -= Create;
            Destroy(this);
        }
    }
}
