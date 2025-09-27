using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public interface ICancel
	{
        public ReactiveValue<bool> CanCancel { get; }
        public void Cancel();
    }
}
