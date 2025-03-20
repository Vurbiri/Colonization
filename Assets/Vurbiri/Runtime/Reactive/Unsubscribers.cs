//Assets\Vurbiri\Runtime\Types\Reactive\Unsubscribers.cs
using System.Collections.Generic;

namespace Vurbiri.Reactive
{
    public class Unsubscribers : Unsubscriber
    {
        private readonly List<Unsubscriber> _unsubscribers;

        public Unsubscribers() => _unsubscribers = new();
        public Unsubscribers(int capacity) => _unsubscribers = new(capacity);
        public Unsubscribers(Unsubscriber unsubscriber)
        {
            _unsubscribers = new() { unsubscriber };
        }

        public override void Unsubscribe()
        {
            for (int i = _unsubscribers.Count - 1; i >= 0; i--)
                _unsubscribers[i]?.Unsubscribe();

            _unsubscribers.Clear();
        }

        public static Unsubscribers operator +(Unsubscribers unsubscribers, Unsubscriber unsubscriber)
        {
            if (unsubscribers == null) return new(unsubscriber);

            unsubscribers._unsubscribers.Add(unsubscriber);
            return unsubscribers;
        }
    }
}
