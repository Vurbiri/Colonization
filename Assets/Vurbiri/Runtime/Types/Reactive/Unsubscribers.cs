using System.Collections.Generic;

namespace Vurbiri.Reactive
{
    public class Unsubscribers : IUnsubscriber
    {
        private readonly List<IUnsubscriber> _unsubscribers;

        public Unsubscribers() => _unsubscribers = new();
        public Unsubscribers(int capacity) => _unsubscribers = new(capacity);

        public void Unsubscribe()
        {
            int count = _unsubscribers.Count;
            for (int i = 0; i < count; i++)
                _unsubscribers[i]?.Unsubscribe();

            _unsubscribers.Clear();
        }

        public static Unsubscribers operator +(Unsubscribers unsubscribers, IUnsubscriber unsubscriber)
        {
            unsubscribers._unsubscribers.Add(unsubscriber);
            return unsubscribers;
        }
    }
}
