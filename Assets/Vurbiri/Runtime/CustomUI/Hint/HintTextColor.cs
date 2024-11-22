//Assets\Vurbiri\Runtime\CustomUI\Hint\HintTextColor.cs
using UnityEngine;

namespace Vurbiri.UI
{
    [System.Serializable]
    public class HintTextColor
	{
        [SerializeField] private Color _colorBase = Color.green;
        [Space]
        [SerializeField] private Color _colorPlus = Color.green;
        [SerializeField] private Color _colorMinus = Color.red;

        private string _hexColorBase, _hexColorPlus, _hexColorMinus;

        //private const string TAG_COLOR_FORMAT = "<color={0}>";
        private const string TAG_COLOR_FORMAT_LITE = "<{0}>";

        public Color ColorBase => _colorBase;
        public Color ColorPlus => _colorPlus;
        public Color ColorMinus => _colorMinus;

        public string HexColorBase => _hexColorBase;
        public string HexColorPlus => _hexColorPlus;
        public string HexColorMinus => _hexColorMinus;

        public void Init()
        {
            _hexColorBase = string.Format(TAG_COLOR_FORMAT_LITE, _colorBase.ToHex());
            _hexColorPlus = string.Format(TAG_COLOR_FORMAT_LITE, _colorPlus.ToHex());
            _hexColorMinus = string.Format(TAG_COLOR_FORMAT_LITE, _colorMinus.ToHex());
        }
    }
}
