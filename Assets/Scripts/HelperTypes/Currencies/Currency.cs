using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency
{
    public CurrencyType Type { get; }
    public int Value { get; }

    public Currency(CurrencyType type, int value)
    {  
        Type = type; 
        Value = value; 
    }
}
