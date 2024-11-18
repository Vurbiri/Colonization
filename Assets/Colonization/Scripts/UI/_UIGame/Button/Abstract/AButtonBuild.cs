using System;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    public abstract class AButtonBuild : AHintingButton
    {
        [Space]
        [SerializeField, Range(0.05f, 0.5f)] private float _space = 0.1f;

        private string _hexColorPlus, _hexColorMinus;

        private const int MIN_SIZE = 64, MAX_SIZE = 256;

        public Vector3 LocalPosition { set => _thisTransform.localPosition = value; }

        protected virtual void Init(Vector3 localPosition, ButtonSettings settings, UnityAction action)
        {
            base.Init(localPosition, settings.hint, action, true);

            _button.targetGraphic.color = settings.color;
            _hexColorPlus = settings.hintColors.HexColorPlus;
            _hexColorMinus = settings.hintColors.HexColorMinus;
        }

        protected void SetTextHint(string caption, ACurrencies cash, ACurrencies cost)
        {
            StringBuilder sb = new(cost.Amount > 0 ? MAX_SIZE : MIN_SIZE);
            sb.AppendLine(caption);
            sb.AppendFormat(CultureInfo.InvariantCulture, TAG_SPACE, _space);

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
            sb.Append(TAG_SPACE_OFF);

            _text = sb.ToString();
        }

    }
}
