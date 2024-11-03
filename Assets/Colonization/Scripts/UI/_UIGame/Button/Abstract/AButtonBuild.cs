using System;
using System.Globalization;
using System.Text;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    public abstract class AButtonBuild : AHinting
    {
        [Space]
        [SerializeField] private Color32 _colorPlus = Color.green;
        [SerializeField] private Color32 _colorMinus = Color.red;
        [Space]
        [SerializeField, Range(0.05f, 0.5f)] private float _space = 0.1f;

        private string _hexColorPlus, _hexColorMinus;

        private const int MIN_SIZE = 64, MAX_SIZE = 256;

        public Vector3 LocalPosition { set => _thisTransform.localPosition = value; }

        protected virtual void Init(Vector3 localPosition, Color color, UnityEngine.Events.UnityAction action)
        {
            base.Init(localPosition, action, true);

            _button.targetGraphic.color = color;
            _hexColorPlus = string.Format(TAG_COLOR_FORMAT_LITE, _colorPlus.ToHex());
            _hexColorMinus = string.Format(TAG_COLOR_FORMAT_LITE, _colorMinus.ToHex());
        }

        protected void SetTextHint(string caption, ACurrencies cash, ACurrencies cost)
        {
            StringBuilder sb = new(cost.Amount > 0 ? MAX_SIZE : MIN_SIZE);
            sb.Append(caption);
            sb.Append(NEW_LINE);
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
