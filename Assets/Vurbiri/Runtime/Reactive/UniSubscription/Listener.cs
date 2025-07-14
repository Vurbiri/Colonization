using System;

namespace Vurbiri.Reactive
{
    [Serializable]
    sealed public class Listener : AListener<Action> { public Listener(Action action) : base(action){ }}
    //=======================================================================================
    [Serializable]
    sealed public class Listener<T> : AListener<Action<T>> { public Listener(Action<T> action) : base(action) { } }
    //=======================================================================================
    [Serializable]
    sealed public class Listener<TA, TB> : AListener<Action<TA, TB>> { public Listener(Action<TA, TB> action) : base(action) { } }
    //=======================================================================================
    [Serializable]
    sealed public class Listener<TA, TB, TC> : AListener<Action<TA, TB, TC>> { public Listener(Action<TA, TB, TC> action) : base(action) { } }
    //=======================================================================================
    [Serializable]
    sealed public class Listener<TA, TB, TC, TD> : AListener<Action<TA, TB, TC, TD>> { public Listener(Action<TA, TB, TC, TD> action) : base(action) { } }
}
