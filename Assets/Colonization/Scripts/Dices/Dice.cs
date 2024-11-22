//Assets\Colonization\Scripts\Dices\Dice.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Dice
    {
        public const int MIN = 1, MAX = 5;

        private readonly int _maxRand = MAX + 1;

        public int Roll() => Random.Range(MIN, _maxRand);
    }
}
