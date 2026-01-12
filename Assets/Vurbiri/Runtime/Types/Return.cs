namespace Vurbiri
{
    public readonly struct Return<T>
    {
        public readonly static Return<T> Empty = new(false, default);

        public readonly bool Result;
        public readonly T Value;

        public Return(T value)
        {
            Result = true;
            Value = value;
        }
        public Return(bool result, T value)
        {
            Result = result;
            Value = value;
        }
    }
}
