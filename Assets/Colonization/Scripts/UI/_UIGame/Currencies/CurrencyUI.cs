using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    public class CurrencyUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _iconTMP;
        [SerializeField] private TMP_Text _countTMP;
        [Space]
        [SerializeField] private RectTransform _thisRectTransform;

        public Vector2  Size => _thisRectTransform.sizeDelta;
        
        public CurrencyUI Init(int id, Vector3 position)
        {
            _iconTMP.text = string.Format(TAG_SPRITE, id);
            _thisRectTransform.transform.localPosition = position;
            return this;
        }

        public void SetValue(int count)
        {
            _countTMP.text = count.ToString();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_thisRectTransform == null)
                _thisRectTransform = GetComponent<RectTransform>();
        }
#endif
    }
}
