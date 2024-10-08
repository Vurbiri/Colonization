using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public class CurrenciesPanel : MonoBehaviour
    {
        [SerializeField] private Id<PlayerId> _playerId;
        [Space]
        [SerializeField] private Currency _currencyUIPrefab;
        [SerializeField] private Amount _amountUI;
        [SerializeField] private Blood _bloodUI;
        [Space]
        [SerializeField] private Vector2 _padding = new(17f, 17f);
        [SerializeField] private float _space = 15f;
        [Space]
        [SerializeField, Range(0.1f, 2f)] private float _scale = 1f;
        [Space]
        [SerializeField] private Vector3 _offsetPopup = Vector3.up * 100f;
        [Space]
        [SerializeField] private RectTransform _thisRectTransform;

        private readonly IdArray<CurrencyId, Currency> _currenciesUI = new();

        private void Start()
        {
            Vector2 cSize = _currencyUIPrefab.Size, aSize = _amountUI.Size, bSize = _bloodUI.Size;
            float offset = cSize.x * 5f + aSize.x + bSize.x + _space * 7f;
            Vector2 size = new()
            {
                y = Mathf.Max(Mathf.Max(cSize.y, aSize.y), bSize.y) + _padding.y * 2f,
                x = offset + _padding.x * 2f
            };

            _thisRectTransform.sizeDelta = size;

            Vector2 pivot = _thisRectTransform.pivot;
            float posX = cSize.x * 0.5f + _padding.x * (1f - 2f * pivot.x) - offset * pivot.x;
            Vector3 pos = new(posX, size.y * (0.5f - pivot.y), 0f);

            offset = cSize.x + _space;
            for (int i = 0; i < CurrencyId.CountMain; i++)
            {
                _currenciesUI[i] = Instantiate(_currencyUIPrefab, _thisRectTransform).Init(i, pos, _offsetPopup);
                pos.x += offset;
            }

            pos.x -= (cSize.x - aSize.x) * 0.5f;
            _amountUI = Instantiate(_amountUI, _thisRectTransform).Init(pos);

            pos.x += (bSize.x + aSize.x) * 0.5f + _space * 2f;
            _bloodUI = Instantiate(_bloodUI, _thisRectTransform).Init(pos, _offsetPopup);

            //transform.localScale = Vector3.one * _scale;

            EventBus.Instance.EventEndSceneCreate += OnEndSceneCreate;
        }

        private void OnEndSceneCreate()
        {
            EventBus.Instance.EventEndSceneCreate -= OnEndSceneCreate;
            Player player = Players.Instance[_playerId];
            var curr = player.Resources;
            Currency currency;
            for (int i = 0; i < CurrencyId.CountMain; i++)
            {
                currency = _currenciesUI[i];
                currency.Unsubscriber = curr.Subscribe(i, currency.SetValue);
            }

            _amountUI.SetReactive(curr, player.GetStateReactive(PlayerStateId.MaxResources));
            _bloodUI.SetReactive(curr, player.GetStateReactive(PlayerStateId.ShrineMaxRes));

            Destroy(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_thisRectTransform == null)
                _thisRectTransform = GetComponent<RectTransform>();
        }
#endif
    }
}
