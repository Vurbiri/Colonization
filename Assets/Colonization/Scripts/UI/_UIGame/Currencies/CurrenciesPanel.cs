using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public class CurrenciesPanel : MonoBehaviour
    {
        [SerializeField] private Id<PlayerId> _playerId;
        [Space]
        [SerializeField] private CurrencyUI _currencyUIPrefab;
        [Space]
        [SerializeField] private Vector2 _padding = new(17f, 17f);
        [SerializeField] private float _space = 15f;
        [Space]
        [SerializeField] private RectTransform _thisRectTransform;

        private readonly IdArray<CurrencyId, CurrencyUI> _currenciesUI = new();
        private readonly List<Unsubscriber<int>> _unsubscribers = new(CurrencyId.Count + 2);

        private void Start()
        {
            
            Vector2 curSize = _currencyUIPrefab.Size;
            float offset = curSize.x * 5f + _space * 4f;
            Vector2 size = new();
            size.y = curSize.y + _padding.y * 2f;
            size.x = offset + _padding.x * 2f;

            _thisRectTransform.sizeDelta = size;

            Vector3 pos = new((curSize.x - offset) / 2f, size.y / 2f, 0f);
            Debug.Log(pos);
            offset = curSize.x + _space;
            for (int i = 0; i < CurrencyId.Count - 1; i++)
            {
                _currenciesUI[i] = Instantiate(_currencyUIPrefab, _thisRectTransform).Initialize(i, pos);
                pos.x += offset;
            }

            EventBus.Instance.EventEndSceneCreate += OnEndSceneCreate;

            //Local
            void OnEndSceneCreate()
            {
                EventBus.Instance.EventEndSceneCreate -= OnEndSceneCreate;
                Currencies curr = Players.Instance[_playerId].Resources;
                for (int i = 0; i < curr.CountMain; i++)
                    _unsubscribers.Add(curr.Subscribe(i, _currenciesUI[i].SetValue));
            }
        }


        private void OnDestroy()
        {
            foreach (var unsubscriber in _unsubscribers)
                unsubscriber?.Unsubscribe();
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
