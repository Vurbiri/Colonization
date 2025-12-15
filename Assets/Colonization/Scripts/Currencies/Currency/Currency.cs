using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public class Currency : ACurrency
    {
        [Impl(256)] public Currency(int value) => _value = value;

        [Impl(256)] public int Set(int value) => Change(value - _value);
        [Impl(256)] public int Add(int delta) => Change(delta);
        [Impl(256)] public int Remove(int delta) => Change(- delta);

        private int Change(int delta)
        {
            if (delta != 0)
            {
                _value += delta;

                Throw.IfNegative(_value);

                _onChange.Invoke(_value);
                _deltaValue.Invoke(delta);
            }
            return delta;
        }
    }
}
