using TMPro;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    public class Currency : MonoBehaviour
    {
        [SerializeField] private TMP_Text _iconTMP;
        [SerializeField] private TMP_Text _countTMP;
        [SerializeField] private CurrencyPopUP _popup;
        [Space]
        [SerializeField] private RectTransform _thisRectTransform;

        private Unsubscriber<int> _unsubscriber;
        
        public Vector2 Size => _thisRectTransform.sizeDelta;
        public Unsubscriber<int> Unsubscriber { set => _unsubscriber = value; }

        public Currency Init(int id, Vector3 position, Vector3 offsetPopup)
        {
            _popup.Init(offsetPopup);
            _iconTMP.text = string.Format(TAG_SPRITE, id);
            _thisRectTransform.transform.localPosition = position;
            return this;
        }

        public void SetValue(int count)
        {
            _popup.Run(count);
            _countTMP.text = count.ToString();
        }

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_thisRectTransform == null)
                _thisRectTransform = GetComponent<RectTransform>();
            if(_popup == null)
                _popup = GetComponentInChildren<CurrencyPopUP>();
        }
#endif
    }
}
