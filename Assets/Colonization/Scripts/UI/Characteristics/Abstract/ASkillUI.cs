using System;

namespace Vurbiri.Colonization.UI
{
	public abstract class ASkillUI : IDisposable
    {
        protected const int SIZE = 78;

        protected readonly int _cost;
        protected readonly string _hexColorPlus, _hexColorMinus;
        protected readonly SeparatorEffectUI _separator;
        protected string _textMain, _textAP;

        public int Cost => _cost;

        public ASkillUI(ProjectColors colors, SeparatorEffectUI separator, int cost)
        {
            _cost = cost;

            _hexColorPlus = colors.TextPositiveTag;
            _hexColorMinus = colors.TextNegativeTag;

            _separator = separator;
        }

        public string GetText(bool isUse) => string.Concat(_textMain, isUse ? _hexColorPlus : _hexColorMinus, _textAP);

        public abstract void Dispose();
    }
}
