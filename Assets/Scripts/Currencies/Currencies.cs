

[System.Serializable]
public class Currencies : EnumArray<Resource, int>
{
    public Currencies() : base() { }
    public Currencies(Currencies other) : base(other) { }

    public void Pay(Currencies cost)
    {
        for (int i = 0; i < _count; i++)
            _values[i] -= cost._values[i];
    }

    public static bool operator >(Currencies left, Currencies right) => !(left <= right);
    public static bool operator <(Currencies left, Currencies right) => !(left >= right);
    public static bool operator >=(Currencies left, Currencies right)
    {
        for (int i = 0; i < left._count; i++)
            if (left._values[i] < right._values[i])
                return false;
        return true;
    }
    public static bool operator <=(Currencies left, Currencies right)
    {
        for(int i = 0; i < left._count; i++ )
            if(left._values[i] > right._values[i]) 
                return false;
        return true;
    }
    
    public override void OnBeforeSerialize()
    {
        base.OnBeforeSerialize();

        for(int i = 0; i < _count; i++)
            if (_values[i] < 0)
                _values[i] = 0;
    }
}
