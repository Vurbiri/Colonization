using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class ProjectColors
	{
        [Header("┌──────────── Panel ─────────────────────")]
        [SerializeField] private Color _panelBack = Color.blue;
        [SerializeField] private Color _panelText = Color.white;
        [Header("├──────────── Hint ─────────────────────"), Space]
        [SerializeField] private Color _hintBack = new(0.985f, 0.882f, 0.725f);
        [SerializeField] private Color _hintDefault = Color.black;
        [Header("├──────────── Text ─────────────────────"), Space]
        [SerializeField] private Color _textDefault = Color.blue;
        [SerializeField] private Color _textPositive = Color.green;
        [SerializeField] private Color _textNegative = Color.red;
        [Header("└────────────────────────────────────"), Space]
#pragma warning disable 414
        [SerializeField, ReadOnly] private string _endColors = "   Цвета проекта   ";
#pragma warning restore 414

        private string _panelTextTag;
        private string _hintDefaultTag;
        private string _textDefaultTag, _textPositiveTag, _textNegativeTag;

        //private const string TAG_COLOR_FORMAT = "<color={0}>";
        private const string TAG_COLOR_FORMAT_LITE = "<{0}>";

        public Color PanelBack => _panelBack;
        public Color PanelText => _panelText;
        public string PanelTextTag => _panelTextTag;

        public Color HinBack => _hintBack;
        public Color HintDefault => _hintDefault;
        public string HintDefaultTag => _hintDefaultTag;

        public Color TextDefault => _textDefault;
        public Color TextPositive => _textPositive;
        public Color TextNegative => _textNegative;
               
        public string TextDefaultTag => _textDefaultTag;
        public string TextPositiveTag => _textPositiveTag;
        public string TextNegativeTag => _textNegativeTag;

        public ProjectColors Init()
        {
            _panelTextTag = string.Format(TAG_COLOR_FORMAT_LITE, _panelText.ToHex());

            _hintDefaultTag = string.Format(TAG_COLOR_FORMAT_LITE, _hintDefault.ToHex());

            _textDefaultTag = string.Format(TAG_COLOR_FORMAT_LITE, _textDefault.ToHex());
            _textPositiveTag = string.Format(TAG_COLOR_FORMAT_LITE, _textPositive.ToHex());
            _textNegativeTag = string.Format(TAG_COLOR_FORMAT_LITE, _textNegative.ToHex());

            return this;
        }

        public Color GetColor(bool isPositive) => isPositive ? _textPositive : _textNegative;
        public string GetHexColor(bool isPositive) => isPositive ? _textPositiveTag : _textNegativeTag;
    }
}
