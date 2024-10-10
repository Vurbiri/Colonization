using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Localization;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    [RequireComponent(typeof(CmButton))]
    public class ButtonUpgrade : AHinting
    {
        [Space]
        [SerializeField] private Color32 _colorPlus = Color.green;
        [SerializeField] private Color32 _colorMinus = Color.red;
        [Space]
        [SerializeField] private Image _buttonIcon;
        [Space]
        [SerializeField] private IdArray<EdificeId, View> _edificeView;

        private CmButton _button;
        private Language _localization;
        private string _hexColorPlus, _hexColorMinus;

        public CmButton Button => _button;

        public override void Init()
        {
            _thisTransform = transform;
            _button = GetComponent<CmButton>();
            _localization = Language.Instance;

            _hexColorPlus = string.Format(TAG_COLOR_FORMAT_LITE, _colorPlus.ToHex());
            _hexColorMinus = string.Format(TAG_COLOR_FORMAT_LITE, _colorMinus.ToHex());
        }

        public void SetupHint(int edificeId, ACurrencies cash, ACurrencies cost)
        {
            View view = _edificeView[edificeId];

            _buttonIcon.sprite = view.sprite;

            StringBuilder sb = new(cost.Amount > 0 ? 180 : 40);
            sb.Append(_localization.GetText(_file, view.key));
            sb.Append(NEW_LINE);

            int costV;
            for (int i = 0; i < CurrencyId.CountMain; i++)
            {
                costV = cost[i];
                if (costV <= 0)
                    continue;

                sb.Append(TAG_COLOR_WHITE);
                sb.AppendFormat(TAG_SPRITE, i);
                sb.Append(costV > cash[i] ? _hexColorMinus : _hexColorPlus);
                sb.Append(costV.ToString());
                sb.Append(SPACE);
            }

            Debug.Log($"{sb.Length}");
            _text = sb.ToString();
        }

        public void AddListener(UnityEngine.Events.UnityAction action) => _button.onClick.AddListener(action);

        protected override void SetText(Language localization) {}

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            for (int i = 0; i < EdificeId.Count; i++)
            {
                if(string.IsNullOrEmpty(_edificeView[i].key))
                _edificeView[i].key = EdificeId.Names[i];
            }
        }
#endif

        #region Nested: View
        //*******************************************************
        [System.Serializable]
        private class View
        {
            public Sprite sprite;
            public string key;
        }
        #endregion
    }
}
