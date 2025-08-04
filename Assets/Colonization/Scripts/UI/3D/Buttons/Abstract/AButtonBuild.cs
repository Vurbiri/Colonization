using System;
using System.Text;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    using static TAG;

    public abstract class AButtonBuild : AHintButton3D
    {
        private string _hexColorPlus, _hexColorMinus;

        private const int MIN_SIZE = 64, MAX_SIZE = 364;

        public Vector3 LocalPosition { set => _rectTransform.localPosition = value; }

        protected virtual void Init(Action action)
        {
            base.Init(GameContainer.UI.WorldHint, action, true);

            _hexColorPlus = GameContainer.UI.Colors.TextPositiveTag;
            _hexColorMinus = GameContainer.UI.Colors.TextNegativeTag;
        }

        protected void SetTextHint(string caption, ACurrencies cash, ACurrencies cost)
        {
            StringBuilder sb = new(cost.Amount > 0 ? MAX_SIZE : MIN_SIZE);
            sb.AppendLine(caption);

            int costV;
            for (int i = 0; i < CurrencyId.MainCount; i++)
            {
                costV = cost[i];
                if (costV > 0)
                    sb.AppendFormat(COLOR_CURRENCY, i, costV.ToString(), costV > cash[i] ? _hexColorMinus : _hexColorPlus);
            }

            _text = sb.ToString();
        }

    }
}
