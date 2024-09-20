using System;
using UnityEngine;

namespace Vurbiri
{
    public abstract class AReactiveValueMono<T> : MonoBehaviour, IReactiveValue<T> where T : AReactiveValueMono<T>
    {
        protected Action<T> EventValueChange;

        event Action<T> IReactiveValue<T>.EventValueChange
        {
            add { EventValueChange += value; }
            remove { EventValueChange -= value; }
        }
    }
}
