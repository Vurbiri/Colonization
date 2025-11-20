using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI
        {
            protected abstract class Actors
            {
                protected readonly List<Actor> _list = new(3);

                public int Count { [Impl(256)] get => _list.Count; }
                public bool NotEmpty { [Impl(256)] get => _list.Count > 0; }


                [Impl(256)] public Actor Extract() => _list.RandomExtract();
                [Impl(256)] public void Clear() => _list.Clear();

                [Impl(256)]  public static implicit operator List<Actor>(Actors self) => self._list;
            }
        }
    }
}
