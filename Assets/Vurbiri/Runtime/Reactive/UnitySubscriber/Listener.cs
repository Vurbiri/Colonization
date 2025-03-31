//Assets\Vurbiri\Runtime\Reactive\UnitySubscriber\Listener.cs
using System;

namespace Vurbiri.Reactive
{
    [Serializable]
    sealed public class Listener : AListener<Action> { }
    //=======================================================================================
    [Serializable]
    sealed public class Listener<T> : AListener<Action<T>> { }
    //=======================================================================================
    [Serializable]
    sealed public class Listener<TA, TB> : AListener<Action<TA, TB>> { }
    //=======================================================================================
    [Serializable]
    sealed public class Listener<TA, TB, TC> : AListener<Action<TA, TB, TC>> { }
    //=======================================================================================
    [Serializable]
    sealed public class Listener<TA, TB, TC, TD> : AListener<Action<TA, TB, TC, TD>> { }
}
