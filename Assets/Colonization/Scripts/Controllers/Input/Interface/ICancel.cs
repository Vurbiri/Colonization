using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public interface ICancel
	{
        public Reactive<bool> CanCancel { get; }
        public void Cancel();
    }
}
