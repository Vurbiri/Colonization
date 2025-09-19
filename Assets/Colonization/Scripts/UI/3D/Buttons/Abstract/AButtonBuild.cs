using System.Text;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    using static TAG;

    public abstract class AButtonBuild : AHintButton3D
    {
        private const int MIN_SIZE = 64, MAX_SIZE = 364;

        public Vector3 LocalPosition { set => _thisRectTransform.localPosition = value; }

        protected void SetTextHint(string caption, ACurrencies cash, ACurrencies cost)
        {
            StringBuilder sb = new(cost.Amount > 0 ? MAX_SIZE : MIN_SIZE);
            sb.AppendLine(caption);

            int costV;
            for (int i = 0; i < CurrencyId.MainCount; i++)
            {
                costV = cost[i];
                if (costV > 0)
                    sb.AppendFormat(COLOR_CURRENCY, i, costV.ToString(), GameContainer.UI.Colors.GetHexColor(costV > cash[i]));
            }

            _hintText = sb.ToString();
        }

    }
}
