using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

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
        [SerializeField] private Color _hintText;
        [Header("├──────────── Text ─────────────────────"), Space]
        [SerializeField] private Color _textDefault;
        [SerializeField] private Color _textDark;
        [SerializeField] private Color _textWarning;
        [SerializeField] private Color _textPositive;
        [SerializeField] private Color _textNegative;

        private string _panelTextTag;
        private string _hintTextTag;

        private string _textDefaultTag, _textWarningTag, _textPositiveTag, _textNegativeTag;

        public ReadOnlyIdArray<ActorAbilityId, Color> Ability => _abilities;

        public Color PanelBack     { [Impl(256)] get => _panelBack; }
        public Color PanelText     { [Impl(256)] get => _panelText; }
        public string PanelTextTag { [Impl(256)] get => _panelTextTag; }

        public Color HintBack     { [Impl(256)] get => _hintBack; }
        public Color HintText     { [Impl(256)] get => _hintText; }
        public string HintTextTag { [Impl(256)] get => _hintTextTag; }

        public Color TextDefault  { [Impl(256)] get => _textDefault; }
        public Color TextDark     { [Impl(256)] get => _textDark; }
        public Color TextWarning  { [Impl(256)] get => _textWarning; }
        public Color TextPositive { [Impl(256)] get => _textPositive; }
        public Color TextNegative { [Impl(256)] get => _textNegative; }

        public string TextDefaultTag  { [Impl(256)] get => _textDefaultTag; }
        public string TextWarningTag  { [Impl(256)] get => _textWarningTag; }
        public string TextPositiveTag { [Impl(256)] get => _textPositiveTag; }
        public string TextNegativeTag { [Impl(256)] get => _textNegativeTag; }

        public ProjectColors Init()
        {
            _panelTextTag = string.Format(TAG.COLOR, _panelText.ToHex());
            _hintTextTag  = string.Format(TAG.COLOR, _hintText.ToHex());

            _textDefaultTag  = string.Format(TAG.COLOR, _textDefault.ToHex());
            _textWarningTag  = string.Format(TAG.COLOR, _textWarning.ToHex());
            _textPositiveTag = string.Format(TAG.COLOR, _textPositive.ToHex());
            _textNegativeTag = string.Format(TAG.COLOR, _textNegative.ToHex());

            return this;
        }

        [Impl(256)] public Color GetTextColor(bool isPositive) => isPositive ? _textPositive : _textNegative;
        [Impl(256)] public string GetHexColor(bool isPositive) => isPositive ? _textPositiveTag : _textNegativeTag;
    }
}
