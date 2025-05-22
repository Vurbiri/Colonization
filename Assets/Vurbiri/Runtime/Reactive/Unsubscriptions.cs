//Assets\Vurbiri\Runtime\Reactive\Unsubscriptions.cs
using System.Collections.Generic;

namespace Vurbiri.Reactive
{
    public class Unsubscriptions : Unsubscription
    {
        private readonly List<Unsubscription> _unsubscribers;

        public Unsubscriptions() => _unsubscribers = new();
        public Unsubscriptions(int capacity) => _unsubscribers = new(capacity);
        public Unsubscriptions(Unsubscription unsubscriber)
        {
            _unsubscribers = new() { unsubscriber };
        }

        public override void Unsubscribe()
        {
            for (int i = _unsubscribers.Count - 1; i >= 0; i--)
                _unsubscribers[i]?.Unsubscribe();

            _unsubscribers.Clear();
        }

        public static Unsubscriptions operator +(Unsubscriptions unsubscribers, Unsubscription unsubscriber)
        {
            if (unsubscribers == null) return new(unsubscriber);

            unsubscribers._unsubscribers.Add(unsubscriber);
            return unsubscribers;
        }
    }
}
