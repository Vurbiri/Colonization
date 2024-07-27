using System;

namespace Vurbiri
{
    public interface IValueTypeEnum<TType> where TType : Enum
    {
        public TType Type { get; }
    }
}
