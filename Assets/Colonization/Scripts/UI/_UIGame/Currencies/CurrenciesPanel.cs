using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    public class CurrenciesPanel : MonoBehaviour
    {
        [SerializeField] private Id<PlayerId> _playerId;
        [Space]
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
            EventBus.Instance.EventEndSceneCreate += Create;
        }

        private void Create()
        {
            RectTransform thisRectTransform = GetComponent<RectTransform>();

            Player player = Players.Instance[_playerId];
            var curr = player.Resources;
            
            GetComponent<Image>().color = player.Color.SetAlpha(_transparency);

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
                Instantiate(_currencyUIPrefab, thisRectTransform).Init(i, pos, curr, _directionPopup);
                pos.x += offset;
            }

            pos.x -= (cSize.x - aSize.x) * 0.5f;
            Instantiate(_amountUIPrefab, thisRectTransform).Init(pos, curr, player.GetStateReactive(PlayerStateId.MaxResources));

            pos.x += (bSize.x + aSize.x) * 0.5f + _space * 2f;
            Instantiate(_bloodUIPrefab, thisRectTransform).Init(pos, curr, player.GetStateReactive(PlayerStateId.ShrineMaxRes), _directionPopup);
            
            EventBus.Instance.EventEndSceneCreate -= Create;
            Destroy(this);
        }
    }
}
