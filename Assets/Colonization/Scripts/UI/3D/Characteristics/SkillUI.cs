using System;
using System.Text;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI_LNG_KEYS;

    [System.Serializable]
    public class SkillUI : IDisposable
    {
        [SerializeField] private int _idNameKey;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _cost;

        private const int SIZE = 64;

        private AEffectsUI[] _effectsTarget;
        private AEffectsUI[] _effectsSelf;
        private string _textMain, _textAP;
        private string _hexColor, _hexColorPlus, _hexColorMinus;
        private int _capacity;

        private Unsubscription _unsubscriber;

        public Sprite Sprite => _sprite;
        public int Cost => _cost;

        public void Init(ProjectColors colors, AEffectsUI[] effectsTarget, AEffectsUI[] effectsSelf)
        {
            _hexColor = colors.HintDefaultTag;
            _hexColorPlus = colors.TextPositiveTag;
            _hexColorMinus = colors.TextNegativeTag;

            _effectsTarget = effectsTarget;
            _effectsSelf = effectsSelf;

            _unsubscriber = Localization.Instance.Subscribe(SetTexts);
        }

        public string GetText(bool isUse)
        {
            StringBuilder sb = new(_textMain, _capacity);
            sb.Append(isUse ? _hexColorPlus : _hexColorMinus);
            sb.Append(_textAP);

            return sb.ToString();
        }

        private void SetTexts(Localization localization)
        {
            int countTarget = _effectsTarget.Length, countSelf = _effectsSelf.Length;

            StringBuilder sb = new(SIZE + countTarget * SIZE + countSelf * SIZE);
            sb.AppendLine(localization.GetText(FILE, KEYS_NAME_SKILLS[_idNameKey]));

            if (countTarget > 0)
            {
                if (countSelf > 0)
                    sb.AppendLine(localization.GetText(FILE, ON_TARGET));

                for (int i = 0; i < countTarget; i++)
                    _effectsTarget[i].GetText(localization, sb);
            }

            if (countSelf > 0)
            {
                if (countTarget > 0)
                {
                    sb.Append(_hexColor);
                    sb.AppendLine(localization.GetText(FILE, ON_SELF));
                }

                for (int i = 0; i < countSelf; i++)
                    _effectsSelf[i].GetText(localization, sb);
            }

            _textMain = sb.ToString();
            _textAP = localization.GetFormatText(FILE, AP_KEY, _cost);

            _capacity = _textMain.Length + +_hexColorPlus.Length + _textAP.Length;
        }

        public void Dispose()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
