using System;

namespace Vurbiri.Colonization.UI
{
	public abstract class ASkillUI : IDisposable
    {
        protected const int SIZE = 78;

        protected readonly string _cost;
        protected readonly SeparatorEffectUI _separator;
        protected string _textMain, _textAP;

        public ASkillUI(SeparatorEffectUI separator, int cost)
        {
            _cost = cost.ToString();
            _separator = separator;
        }

        public string GetText(bool isUse) => string.Concat(_textMain, GameContainer.UI.Colors.GetHexColor(isUse), _textAP);

        public abstract void Dispose();
    }
}
