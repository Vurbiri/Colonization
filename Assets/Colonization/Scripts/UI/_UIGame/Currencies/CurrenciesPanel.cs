//Assets\Colonization\Scripts\UI\_UIGame\Currencies\CurrenciesPanel.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{

    public class CurrenciesPanel : MonoBehaviour
    {
        [SerializeField] private Currency _currencyUIPrefab;
        [SerializeField] private Amount _amountUIPrefab;
        [SerializeField] private Blood _bloodUIPrefab;
        [Space]
        [SerializeField] private Vector2 _padding = new(17f, 17f);
        [SerializeField] private float _space = 15f;
        [SerializeField] private float _transparency = 0.84f;
        [Space]
        [SerializeField] private Direction2 _directionPopup;

        private void Start()
        {
            SceneContainer.Get<GameplayEventBus>().EventSceneEndCreation += Create;
        }

        private void Create()
        {
            RectTransform thisRectTransform = GetComponent<RectTransform>();

            GetComponent<Image>().color = SceneContainer.Get<PlayersVisual>()[PlayerId.Player].color.SetAlpha(_transparency);

            var currencies = SceneContainer.Get<Players>().Player.Resources;
            var settings = SceneContainer.Get<Vurbiri.UI.TextColorSettings>();

            Vector2 cSize = _currencyUIPrefab.Size, aSize = _amountUIPrefab.Size, bSize = _bloodUIPrefab.Size;
            float offset = cSize.x * 5f + aSize.x + bSize.x + _space * 7f;

            Vector2 size = new()
            {
                y = Mathf.Max(Mathf.Max(cSize.y, aSize.y), bSize.y) + _padding.y * 2f,
                x = offset + _padding.x * 2f
            };
            thisRectTransform.sizeDelta = size;

            Vector2 pivot = thisRectTransform.pivot;
            float posX = cSize.x * 0.5f + _padding.x - size.x * pivot.x;
            float posY = size.y * (0.5f - pivot.y);
            Vector3 pos = new(posX, posY, 0f);

            offset = cSize.x + _space;
            for (int i = 0; i < CurrencyId.CountMain; i++)
            {
                Instantiate(_currencyUIPrefab, thisRectTransform).Init(i, pos, currencies, settings, _directionPopup);
                pos.x += offset;
            }

            pos.x -= (cSize.x - aSize.x) * 0.5f;
            Instantiate(_amountUIPrefab, thisRectTransform).Init(pos, currencies.AmountCurrent, currencies.AmountMax, settings);

            pos.x += (bSize.x + aSize.x) * 0.5f + _space * 2f;
            Instantiate(_bloodUIPrefab, thisRectTransform).Init(pos, currencies.BloodCurrent, currencies.BloodMax, settings, _directionPopup);

            SceneContainer.Get<GameplayEventBus>().EventSceneEndCreation -= Create;
            Destroy(this);
        }
    }
}
