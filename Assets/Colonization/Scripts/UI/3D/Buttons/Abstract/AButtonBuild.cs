using System;
using System.Globalization;
using System.Text;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    public abstract class AButtonBuild : AHintButton3D
    {
        private string _hexColorPlus, _hexColorMinus;

        private const int MIN_SIZE = 64, MAX_SIZE = 256;
        private const float SPACE = 0.1f;

        public Vector3 LocalPosition { set => _rectTransform.localPosition = value; }

        protected virtual void Init(ButtonSettings settings, Action action)
        {
            base.Init(settings.hint, action, true);

            _hexColorPlus = settings.colorSettings.TextPositiveTag;
            _hexColorMinus = settings.colorSettings.TextNegativeTag;
        }

        protected void SetTextHint(string caption, ACurrencies cash, ACurrencies cost)
        {
            StringBuilder sb = new(cost.Amount > 0 ? MAX_SIZE : MIN_SIZE);
            sb.AppendLine(caption);
            sb.AppendFormat(CultureInfo.InvariantCulture, TAG_CSPACE, SPACE);

            int costV;
            for (int i = 0; i < CurrencyId.MainCount; i++)
            {
                costV = cost[i];
                if (costV <= 0)
                    continue;

                sb.Append(TAG_COLOR_WHITE);
                sb.AppendFormat(TAG_SPRITE, i);
                sb.Append(costV > cash[i] ? _hexColorMinus : _hexColorPlus);
                sb.Append(costV.ToString());
            }
            sb.Append(TAG_CSPACE_OFF);

            _text = sb.ToString();
        }

    }
}
