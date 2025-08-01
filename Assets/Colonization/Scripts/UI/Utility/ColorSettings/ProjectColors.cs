using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using VurbiriEditor.Colonization;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class ProjectColors
	{
        [Header("┌─────────────────────────────────────")]
        [SerializeField] private ReadOnlyIdArray<ActorAbilityId, Color> _abilities;
        [Header("├──────────── Panel ─────────────────────")]
        [SerializeField] private Color _panelBack;
        [SerializeField] private Color _panelText;
        [Header("├──────────── Hint ─────────────────────"), Space]
        [SerializeField] private Color _hintBack;
        [SerializeField] private Color _hintDefault;
        [Header("├──────────── Text ─────────────────────"), Space]
        [SerializeField] private Color _textDefault;
        [SerializeField] private Color _textDark;
        [SerializeField] private Color _textWarning;
        [SerializeField] private Color _textPositive;
        [SerializeField] private Color _textNegative;

        private string _panelTextTag;
        private string _hintDefaultTag;

        private string _textDefaultTag, _textPositiveTag, _textNegativeTag;

        //private const string TAG_COLOR_FORMAT = "<color={0}>";
        private const string TAG_COLOR_FORMAT = "<{0}>";

        public ReadOnlyIdArray<ActorAbilityId, Color> Ability => _abilities;

        public Color PanelBack => _panelBack;
        public Color PanelText => _panelText;
        public string PanelTextTag => _panelTextTag;

        public Color HintBack => _hintBack;
        public Color HintDefault => _hintDefault;
        public string HintDefaultTag => _hintDefaultTag;

        public Color TextDefault => _textDefault;
        public Color TextDark => _textDark;
        public Color TextWarning => _textWarning;
        public Color TextPositive => _textPositive;
        public Color TextNegative => _textNegative;
               
        public string TextDefaultTag => _textDefaultTag;
        public string TextPositiveTag => _textPositiveTag;
        public string TextNegativeTag => _textNegativeTag;

        public ProjectColors Init()
        {
            _panelTextTag = string.Format(TAG_COLOR_FORMAT, _panelText.ToHex());
            _hintDefaultTag = string.Format(TAG_COLOR_FORMAT, _hintDefault.ToHex());

            _textDefaultTag = string.Format(TAG_COLOR_FORMAT, _textDefault.ToHex());
            _textPositiveTag = string.Format(TAG_COLOR_FORMAT, _textPositive.ToHex());
            _textNegativeTag = string.Format(TAG_COLOR_FORMAT, _textNegative.ToHex());

            return this;
        }

        public Color GetTextColor(bool isPositive) => isPositive ? _textPositive : _textNegative;
        public string GetHexColor(bool isPositive) => isPositive ? _textPositiveTag : _textNegativeTag;

#if UNITY_EDITOR
        public void SetColors_Editor(UISettings_Editor.Colors colors)
        {
            _panelBack = colors.panelBack;
            _panelText = colors.panelText;
            _hintBack = colors.hintBack;
            _hintDefault = colors.hintText;
        }
#endif
    }
}
