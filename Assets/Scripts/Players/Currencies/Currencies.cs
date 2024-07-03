using UnityEngine;

[System.Serializable]
public class Currencies : EnumArray<Resource, int>
{
    [SerializeField] private int _amount;
    public int Amount => _amount;
    
    public override int this[Resource type] { get => _values[(int)type]; set => Add(type, value); }
    public override int this[int index] { get => _values[index]; set => Add(index, value); }

    public Currencies() : base() => _amount = 0;
    public Currencies(Currencies other) => CopyFrom(other);

    public void CopyFrom(Currencies other)
    {
        for (int i = 0; i < _count; i++)
            _values[i] = other._values[i];

        _amount = other._amount;
    }

    //TEST
    public void Rand(int max)
    {
        Clear();
        for (int i = 0; i < _count; i++)
            Add(i, Random.Range(0, max + 1));

    }
    //TEST
    public void Clear()
    {
        for (int i = 0; i < _count; i++)
            _values[i] = 0;

        _amount = 0;

    }


    public void Add(int id, int value)
    {
        _values[id] += value;
        _amount += value;
    }
    public void Add(Resource type, int value) => Add(id : (int)type, value);

    public void Pay(Currencies cost)
    {
        for (int i = 0; i < _count; i++)
            _values[i] -= cost._values[i];

        _amount -= cost._amount;
    }

    public static bool operator >(Currencies left, Currencies right) => !(left <= right);
    public static bool operator <(Currencies left, Currencies right) => !(left >= right);
    public static bool operator >=(Currencies left, Currencies right)
    {
        if(left == null || right == null || left._amount < right._amount)
            return false;
        
        for (int i = 0; i < left._count; i++)
            if (left._values[i] < right._values[i])
                return false;
        return true;
    }
    public static bool operator <=(Currencies left, Currencies right)
    {
        if (left == null || right == null)
            return false;

        for (int i = 0; i < left._count; i++ )
            if(left._values[i] > right._values[i]) 
                return false;
        return true;
    }
    
    public override void OnBeforeSerialize()
    {
        base.OnBeforeSerialize();

        _amount = 0;
        for (int i = 0; i < _count; i++)
            _amount += _values[i];
    }
}
