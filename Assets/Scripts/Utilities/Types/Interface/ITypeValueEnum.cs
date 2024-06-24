using System;

public interface ITypeValueEnum<TKey> where TKey : Enum
{
    public TKey Type { get; }
}
