using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    public abstract class AButtonBuild : AHinting
    {
        [Space]
        [SerializeField] private Color32 _colorPlus = Color.green;
        [SerializeField] private Color32 _colorMinus = Color.red;

        protected CmButton _button;
        private CmButton.ButtonClickedEvent _buttonClicked;
        private Graphic _targetGraphic;
        private string _hexColorPlus, _hexColorMinus;

        public CmButton Button => _button;
        public Color Color { set => _targetGraphic.color = value; }

        protected virtual void Init()
        {
            _button = GetComponent<CmButton>();
            _buttonClicked = _button.onClick;
            _targetGraphic = _button.targetGraphic;

            _hexColorPlus = string.Format(TAG_COLOR_FORMAT_LITE, _colorPlus.ToHex());
            _hexColorMinus = string.Format(TAG_COLOR_FORMAT_LITE, _colorMinus.ToHex());
        }

        protected void SetTextHint(string caption, ACurrencies cash, ACurrencies cost)
        {
            _button.Interactable = cash >= cost;

            StringBuilder sb = new(cost.Amount > 0 ? 200 : 50);
            sb.Append(caption);
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

            _text = sb.ToString();
        }

        public void AddListener(UnityEngine.Events.UnityAction action) => _buttonClicked.AddListener(action);
        public void RemoveAllListeners() => _buttonClicked.RemoveAllListeners();

        public void SetActive(bool active) => gameObject.SetActive(active);
    }
}
