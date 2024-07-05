using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice 
{
    public const int MIN = 1, MAX = 5;

    private int _maxRand = MAX + 1;

    public int Roll() => Random.Range(MIN, _maxRand);
}
