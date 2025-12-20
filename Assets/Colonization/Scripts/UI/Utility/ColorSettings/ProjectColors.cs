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
        [Header("┌──────────── Text ─────────────────────"), Space]
        [SerializeField] private Color _textDefault;
        [SerializeField] private Color _textWarning;
        [SerializeField] private Color _textPositive;
        [SerializeField] private Color _textNegative;
        [Space]
        [SerializeField] private string _hintTextTag;

        private string _textDefaultTag, _textWarningTag, _textPositiveTag, _textNegativeTag;

        public ReadOnlyIdArray<ActorAbilityId, Color> Ability { [Impl(256)] get => _abilities; }

        public Color TextDefault  { [Impl(256)] get => _textDefault; }
        public Color TextWarning  { [Impl(256)] get => _textWarning; }
        public Color TextPositive { [Impl(256)] get => _textPositive; }
        public Color TextNegative { [Impl(256)] get => _textNegative; }

        public string TextDefaultTag  { [Impl(256)] get => _textDefaultTag; }
        public string TextWarningTag  { [Impl(256)] get => _textWarningTag; }
        public string TextPositiveTag { [Impl(256)] get => _textPositiveTag; }
        public string TextNegativeTag { [Impl(256)] get => _textNegativeTag; }

        public string HintTextTag { [Impl(256)] get => _hintTextTag; }

        public ProjectColors Init()
        {
            _textDefaultTag  = string.Format(TAG.COLOR, _textDefault.ToHex());
            _textWarningTag  = string.Format(TAG.COLOR, _textWarning.ToHex());
            _textPositiveTag = string.Format(TAG.COLOR, _textPositive.ToHex());
            _textNegativeTag = string.Format(TAG.COLOR, _textNegative.ToHex());

            return this;
        }

        [Impl(256)] public Color GetColor(bool isPositive) => isPositive ? _textPositive : _textNegative;
        [Impl(256)] public string GetHexColor(bool isPositive) => isPositive ? _textPositiveTag : _textNegativeTag;

#if UNITY_EDITOR
        public void SetHintTextTag(Color color) => _hintTextTag = string.Format(TAG.COLOR, color.ToHex());
#endif
    }
}
