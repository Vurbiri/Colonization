//Assets\Colonization\Scripts\Dices\Dices.cs
namespace Vurbiri.Colonization
{
    public class Dices
    {
        private readonly Dice[] _dices = { new(), new(), new() };
        private int _preview = -1, _current = -1;

        public int Preview => _preview;
        public int Current => _current;

        public void Roll()
        {
            _preview = _current;
            _current = _dices[0].Roll() + _dices[1].Roll() + _dices[2].Roll();
        }
    }
}
