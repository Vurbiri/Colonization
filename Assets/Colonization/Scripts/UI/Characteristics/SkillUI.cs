using System.Text;
using UnityEngine;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    public class SkillUI : ASkillUI
    {
        private readonly AEffectUI[] _effectsTarget;
        private readonly AEffectUI[] _effectsSelf;
        private readonly Settings _settings;

        public string Key => _settings.keySkillName;
        public Sprite Sprite => _settings.sprite;


        public SkillUI(SeparatorEffectUI separator, int cost, Settings settings, AEffectUI[] effectsTarget, AEffectUI[] effectsSelf) : base(separator, cost)
        {
            _settings = settings;

            _effectsTarget = effectsTarget;
            _effectsSelf = effectsSelf;

            Localization.Instance.Subscribe(SetTexts);
        }

        private void SetTexts(Localization localization)
        {
            int countTarget = _effectsTarget.Length, countSelf = _effectsSelf.Length;

            StringBuilder sb = new(SIZE + countTarget * SIZE + countSelf * SIZE);
            sb.AppendLine(localization.GetText(FILE, _settings.keySkillName));
            _separator.GetText(sb);

            if (countTarget > 0)
            {
                if (countSelf > 0)
                {
                    sb.Append(GameContainer.UI.Colors.HintTextTag);
                    sb.AppendLine(localization.GetText(FILE, ON_TARGET));
                }

                for (int i = 0; i < countTarget; i++)
                   _effectsTarget[i].GetText(localization, sb);
            }

            if (countSelf > 0)
            {
                sb.Append(GameContainer.UI.Colors.HintTextTag);
                sb.AppendLine(localization.GetText(FILE, ON_SELF));

                for (int i = 0; i < countSelf; i++)
                    _effectsSelf[i].GetText(localization, sb);
            }

            _textMain = sb.ToString();
            _textAP = localization.GetFormatText(FILE, AP_KEY, _cost);
        }

        public override void Dispose()
        {
            Localization.Instance.Unsubscribe(SetTexts);
        }

        // Nested
        //******************************************************************
        [System.Serializable]
        public class Settings
        {
            public string keySkillName;
            public Sprite sprite;
        }
    }
}
