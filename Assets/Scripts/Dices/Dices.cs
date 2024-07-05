using UnityEngine;

public class Dices : MonoBehaviour
{
    private Dice[] _dices = { new(), new(), new() };

    public int Roll() => _dices[0].Roll() + _dices[1].Roll() + _dices[2].Roll();
}
