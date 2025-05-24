namespace Vurbiri
{
    public class Return<T>
    {
        public bool Result { get; }
        public T Value { get; }

        public static Return<T> Empty { get; } = new();

        private Return()
        {
            Result = false;
            Value = default;
        }

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

        public override string ToString() => Value.ToString();
    }
}
