//Assets\Vurbiri\Runtime\Types\Collections\EnumCollections\Interface\IValueTypeEnum.cs
using System;

namespace Vurbiri
{
    public interface IValueTypeEnum<TType> where TType : Enum
    {
        public TType Type { get; }
    }
}
