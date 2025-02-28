//Assets\Vurbiri.UI\Runtime\Utility\TextColorSettings.cs
using UnityEngine;

namespace Vurbiri.UI
{
    [System.Serializable]
    public class TextColorSettings
	{
        [SerializeField] private Color _colorHintBase = Color.black;
        [SerializeField] private Color _colorTextBase = Color.blue;
        [Space]
        [SerializeField] private Color _colorPositive = Color.green;
        [SerializeField] private Color _colorNegative = Color.red;

        private string _hexColorHintBase, _hexColorTextBase, _hexColorPositive, _hexColorNegative;

        //private const string TAG_COLOR_FORMAT = "<color={0}>";
        private const string TAG_COLOR_FORMAT_LITE = "<{0}>";

        public Color ColorHintBase => _colorHintBase;
        public Color ColorTextBase => _colorTextBase;
        public Color ColorPositive => _colorPositive;
        public Color ColorNegative => _colorNegative;

        public string HexColorHintBase => _hexColorHintBase;
        public string HexColorTextBase => _hexColorTextBase;
        public string HexColorPositive => _hexColorPositive;
        public string HexColorNegative => _hexColorNegative;

        public TextColorSettings Init()
        {
            _hexColorTextBase = string.Format(TAG_COLOR_FORMAT_LITE, _colorTextBase.ToHex());
            _hexColorHintBase = string.Format(TAG_COLOR_FORMAT_LITE, _colorHintBase.ToHex());
            _hexColorPositive = string.Format(TAG_COLOR_FORMAT_LITE, _colorPositive.ToHex());
            _hexColorNegative = string.Format(TAG_COLOR_FORMAT_LITE, _colorNegative.ToHex());

            return this;
        }

        public Color GetColor(bool isPositive) => isPositive ? _colorPositive : _colorNegative;
        public string GetHexColor(bool isPositive) => isPositive ? _hexColorPositive : _hexColorNegative;
    }
}
