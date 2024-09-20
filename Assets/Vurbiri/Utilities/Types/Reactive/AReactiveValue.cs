using System;

namespace Vurbiri
{
    public abstract class AReactiveValue<T> : IReactiveValue<T> where T : AReactiveValue<T>
    {
        protected Action<T> EventValueChange;

        event Action<T> IReactiveValue<T>.EventValueChange
        {
            add { EventValueChange += value; }
            remove { EventValueChange -= value; }
        }
    }
}
