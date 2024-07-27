using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Dices : MonoBehaviour
    {
        private readonly Dice[] _dices = { new(), new(), new() };

        public int Roll() => _dices[0].Roll() + _dices[1].Roll() + _dices[2].Roll();
    }
}
